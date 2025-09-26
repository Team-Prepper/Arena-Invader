using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using EasyH;
using EasyH.Unity.UI;
using EasyH.Gaming.TurnBased;

public class UNetPlayground : NetworkBehaviour, IPlayground
{

    [SerializeField] private UNetTurnSystem _turnSystem;

    private ISet<int> _deathPlayerIdx;
    private int _readyPlayerCnt = 0;

    public IList<IPlayableCharacter> Players { get; private set; }

    public override void OnNetworkSpawn()
    {
        GameManager.Instance.Playground = this;
        TurnManager.Instance.System = _turnSystem;

        if (!NetworkManager.Singleton.IsHost)
        {
            Debug.Log("This is Client UNetPlayground");
        }

        Players = new List<IPlayableCharacter>();

        _deathPlayerIdx = new HashSet<int>();

    }

    public IPlayableCharacter NowPlayer
    {
        get
        {
            if (Players == null || TurnManager.Instance.System.
                ActiveTeamIdx >= Players.Count) return null;
            return Players[TurnManager.Instance.System.ActiveTeamIdx];
        }

    }

    public IPlayableCharacter InstantiateCC(Vector3 pos)
    {
        GameObject retval = AssetOpener.ImportGameObject("UNetCC");
        retval.GetComponent<NetworkObject>().Spawn();
        retval.transform.position = pos;

        return retval.GetComponent<IPlayableCharacter>();
    }

    public IStatus ObjectCharacter { get; set; }

    public IStatus InstantiateStatus()
    {
        if (!IsHost) return null;
        GameObject go = AssetOpener.ImportGameObject("UNetOC");
        
        UNetObjectCharacter retval = go.GetComponent<UNetObjectCharacter>();
        retval.GetComponent<NetworkObject>().Spawn();

        retval.SetTargetCharacter("Baron");

        return retval;
    }

    public void StartMatch()
    {

        StartMatchClientRpc();
    }

    [ClientRpc]
    public void StartMatchClientRpc()
    {

        MatchGenerator generator = GameObject.FindWithTag
            ("MatchGenerator").GetComponent<MatchGenerator>();

        generator.SetMatchInfor(GameManager.Instance.MatchInfor);
        generator.GenerateMap();

        if (!NetworkManager.Singleton.IsHost)
        {
            MatchLoadComplete();
            return;
        }

        generator.Generate();

    }

    public void AddPlayer(IPlayableCharacter player)
    {
        if (Players.Contains(player)) return;

        while (Players.Count <= player.TurnState.TeamIdx)
        {
            Players.Add(null);
        }

        Players[player.TurnState.TeamIdx] = player;
    }

    public bool GameProceed()
    {
        return Players.Count - _deathPlayerIdx.Count > 1;
    }

    public void PlayerDeath(IPlayableCharacter player)
    {
        for (int i = 0; i < Players.Count; i++)
        {
            if (Players[i] != player) continue;
            _deathPlayerIdx.Add(i);
            break;
        }

    }

    public void CheckGameEnd()
    {
        if (!IsOwner) return;
        if (GameProceed()) return;

        GameEnd();
    }

    public void MatchLoadComplete()
    {
        PlayReadyServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void PlayReadyServerRpc()
    {

        _readyPlayerCnt++;

        if (_readyPlayerCnt >= GameManager.Instance.MatchInfor.PlayerInfors.Count)
        {
            GameStartClientRpc();
            TurnManager.Instance.System.SetGameProceedCondition(GameProceed);
            TurnManager.Instance.System.StartGame();
        }
    }

    [ClientRpc]
    public void GameStartClientRpc()
    {
        Players[(int)NetworkManager.Singleton.LocalClientId].
            SetController(new GUICharacterController());

        UIManager.Instance.OpenGUI<GUIPlayground>
            ("Playground").Generate();
    }

    void GameEnd()
    {
        GameEndClientRpc();
    }

    [ClientRpc]
    void GameEndClientRpc()
    {
        Destroy(BoardManager.Instance.Map.gameObject);

        BoardManager.Instance.Map = null;

        foreach (var player in Players)
        {
            if (player.Status.IsAlive())
            {
                IGUIFullScreen nowScreen = UIManager.Instance.NowDisplay;
                UIManager.Instance.OpenGUI<GUIResult>("Result").SetWinner(player.Status);
                SFXManager.Instance.PlayBGM("Win");
                nowScreen.Close();
            }
            player.Dispose();
        }
        GameManager.Instance.Playground = new Playground();
        TurnManager.Instance.System = new TurnSystem();
    }

    public int CalcDamage(IStatus attacker, IStatus target)
    {
        return Mathf.Max(1, attacker.Atk - target.Dfs);
    }
    
}