using EHTool;
using EHTool.LangKit;
using EHTool.UIKit;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class UNetPlayground : NetworkBehaviour, IPlayground {

    int _readyPlayerCnt = 0;

    ISet<int> _deathPlayerIdx;
    int _turnIdx;

    bool _actionSelectorSet;

    public GameMap Map { get; set; }
    public int Turn { get; private set; }

    public IList<ICharacterController> Players { get; private set; }

    public override void OnNetworkSpawn()
    {
        GameManager.Instance.Playground = this;

        if (!NetworkManager.Singleton.IsHost)
        {
            Debug.Log("This is Client UNetPlayground");
        }

        Players = new List<ICharacterController>();

        _deathPlayerIdx = new HashSet<int>();
        _turnIdx = 0;
        Turn = 0;
        _actionSelectorSet = false;

    }

    public ICharacterController NowPlayer {
        get {
            if (Players == null || _turnIdx >= Players.Count) return null;
            return Players[_turnIdx];
        }

    }

    public ICharacterController InstantiateCC(Vector3 pos)
    {
        UNetCharacterController retval =
            AssetOpener.ImportComponent<UNetCharacterController>("UNetCC");
        retval.GetComponent<NetworkObject>().Spawn();
        retval.transform.position = pos;

        return retval;
    }

    public IStatus ObjectCharacter { get; set; }

    public IStatus InstantiateStatus()
    {
        if (!IsHost) return null;

        UNetObjectCharacter retval = 
            AssetOpener.ImportComponent<UNetObjectCharacter>("UNetOC");
        retval.GetComponent<NetworkObject>().Spawn();

        retval.SetTargetCharacter("Baron");

        return retval;
    }

    public void StartMatch()
    {
        if (!NetworkManager.Singleton.IsHost) return;

        MatchGenerator generator = GameObject.FindWithTag
            ("MatchGenerator").GetComponent<MatchGenerator>();
        generator.SetMatchInfor(GameManager.Instance.MatchInfor);
        generator.Generate();

        StartMatchClientRpc();
    }

    [ClientRpc]
    public void StartMatchClientRpc() {

        Map = AssetOpener.ImportComponent<GameMap>
            (GameManager.Instance.MatchInfor.MapName);

        if (IsHost) return;

        PlayReady();

    }

    public void AddPlayer(ICharacterController player)
    {
        if (Players.Contains(player)) return;

        while (Players.Count <= player.PlayerId) {
            Players.Add(null);
        }

        Players[player.PlayerId] = player;
    }

    public bool IsGameEnd()
    {
        return Players.Count - _deathPlayerIdx.Count < 2;
    }

    public void PlayerDeath(ICharacterController player)
    {
        for (int i = 0; i < Players.Count; i++)
        {
            if (Players[i] != player) continue;
            _deathPlayerIdx.Add(i);
            break;
        }
    }

    public void PlayReady()
    {
        PlayReadyServerRpc();
    }

    [ServerRpc(RequireOwnership =false)]
    public void PlayReadyServerRpc() {

        _readyPlayerCnt++;

        if (_readyPlayerCnt >= GameManager.Instance.MatchInfor.PlayerInfors.Count) {
            GameStartClientRpc();
            TurnStart();
        }
    }

    [ClientRpc]
    public void GameStartClientRpc()
    {
        UIManager.Instance.OpenGUI<GUIPlayground>("Playground").Generate();
    }

    public void TurnStart()
    {
        TurnStartClientRpc(Turn, _turnIdx);
    }

    [ClientRpc]
    public void TurnStartClientRpc(int turn, int idx)
    {
        Turn = turn;
        Map.StartNewTurn(Turn);

        GUITurnStart turnStartCall = UIManager.Instance.OpenGUI<GUITurnStart>("TurnStart");

        Action callbackAction = () => {
            turnStartCall.Close();
        };

        if (NetworkManager.Singleton.ConnectedClientsIds.ElementAt(idx)
            == NetworkManager.Singleton.LocalClientId)
        {

            if (!_actionSelectorSet)
            {
                Players[idx].SetMatch(new GUICharacterActionSelector());
                _actionSelectorSet = true;
            }

            callbackAction += () => {
                Players[idx].StartTurn();
            };

        }

        turnStartCall.SetWaitForCallback(
            string.Format(LangManager.Instance.GetStringByKey("msg_XTurn"),
            Players[idx].Status.Name), callbackAction);

    }

    public void TurnEnd() {
        TurnEndServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void TurnEndServerRpc()
    {
        if (IsGameEnd())
        {
            GameEnd();
            return;
        }

        while (true)
        {
            _turnIdx = _turnIdx + 1;

            if (_turnIdx >= Players.Count)
            {
                Turn++;
                _turnIdx = 0;
            }
            if (!_deathPlayerIdx.Contains(_turnIdx)) break;
        }

        TurnStart();
    }

    void GameEnd()
    {
        GameEndClientRpc();
    }

    [ClientRpc]
    void GameEndClientRpc() {

        GameManager.Instance.Playground = new LocalPlayground();
        Destroy(Map.gameObject);

        Map = null;

        foreach (var player in Players)
        {
            if (player.Status.IsAlive())
            {
                IGUIFullScreen nowScreen = UIManager.Instance.NowDisplay;
                UIManager.Instance.OpenGUI<GUIResult>("Result").SetWinner(player.Status);
                SFXManager.Instance.PlayBGM("Win");
                nowScreen.Close();
            }
            Destroy(player.Target.gameObject);
        }
    }

    public int CalcDamage(IStatus attacker, IStatus target)
    {
        return Mathf.Max(1, attacker.Atk - target.Dfs);
    }
}