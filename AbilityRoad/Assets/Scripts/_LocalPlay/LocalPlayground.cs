using EHTool;
using EHTool.LangKit;
using EHTool.UIKit;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayground : IPlayground {

    ISet<int> _deathPlayerIdx;
    int _turnIdx;

    public GameMap Map { get; set; }
    public int Turn { get; private set; }

    public IList<ICharacterController> Players { get; private set; }

    public ICharacterController NowPlayer {
        get {
            if (Players == null || _turnIdx >= Players.Count) return null;
            return Players[_turnIdx];
        }

    }

    public LocalPlayground()
    {
        Players = new List<ICharacterController>();

        _deathPlayerIdx = new HashSet<int>();
        _turnIdx = 0;
        Turn = 0;

    }

    public ICharacterController InstantiateCC(Vector3 pos) {
        LocalCharacterController retval =
            AssetOpener.ImportComponent<LocalCharacterController>("LocalCC");
        retval.transform.position = pos;

        return retval;
    }

    public void StartMatch()
    {
        Map = AssetOpener.ImportComponent<GameMap>
            (GameManager.Instance.MatchInfor.MapName);

        MatchGenerator generator = GameObject.FindWithTag("MatchGenerator").GetComponent<MatchGenerator>();
        generator.SetMatchInfor(GameManager.Instance.MatchInfor);
        generator.Generate();

    }

    public void PlayReady()
    {
        UIManager.Instance.OpenGUI<GUIPlayground>("Playground").Generate();

        GUICharacterActionSelector _guiActionSelector = new GUICharacterActionSelector();
        AICharacterActionSelector _aiActionSelector = new GameObject().AddComponent<AICharacterActionSelector>();

        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].SetMatch(
                GameManager.Instance.MatchInfor.PlayerInfors[i].
                CharacterCode.Equals("Player_AI") ?
                _aiActionSelector : _guiActionSelector);

        }

        TurnStart();
    }

    public void AddPlayer(ICharacterController player)
    {
        if (Players.Contains(player)) return;
        Players.Add(player);
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

    void GameEnd()
    {
        GameManager.Instance.Playground = new LocalPlayground();
        Object.Destroy(Map.gameObject);
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
            Object.Destroy(player.Target.gameObject);
        }
    }

    public void TurnStart()
    {
        GUITurnStart turnStartCall = UIManager.Instance.OpenGUI<GUITurnStart>("TurnStart");

        turnStartCall.SetWaitForCallback(
            string.Format(LangManager.Instance.GetStringByKey("msg_XTurn"), Players[_turnIdx].Status.Name), () => {
            Players[_turnIdx].StartTurn();
            turnStartCall.Close();
        });
    }

    public void TurnEnd()
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
                Map.StartNewTurn(Turn);
            }
            if (!_deathPlayerIdx.Contains(_turnIdx)) break;
        }

        TurnStart();
    }

    public int CalcDamage(IStatus attacker, IStatus target)
    {
        return Mathf.Max(1, attacker.Atk - target.Dfs);
    }

    public bool IsGameEnd()
    {
        return Players.Count - _deathPlayerIdx.Count < 2;
    }

}