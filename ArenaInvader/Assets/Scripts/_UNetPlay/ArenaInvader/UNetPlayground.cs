using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using EasyH;
using EasyH.Unity;
using EasyH.Unity.UI;
using EasyH.Gaming.TurnBased;

public class UNetPlayground : NetworkBehaviour, IPlayground
{

    [SerializeField] private UNetTurnSystem _turnSystem;

    private ISet<int> _deathPlayerIdx;
    private int _readyPlayerCnt = 0;
    private bool _matchStarted;

    public IList<IPlayableCharacter> Players { get; private set; }

    public override void OnNetworkSpawn()
    {
        GameManager.Instance.Playground = this;
        TurnManager.Instance.System = _turnSystem;

        if (!NetworkManager.Singleton.IsHost)
        {
            Debug.Log("This is Client UNetPlayground");
        }

        ResetMatchState();
    }

    public override void OnNetworkDespawn()
    {
        ResetMatchState();
        base.OnNetworkDespawn();
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
        return InstantiateCC(pos, NetworkManager.ServerClientId);
    }

    public IPlayableCharacter InstantiateCC(Vector3 pos, ulong ownerClientId)
    {
        GameObject retval = ResourceManager.Instance.ResourceConnector.ImportGameObject("UNetCC");
        NetworkObject networkObject = retval.GetComponent<NetworkObject>();
        networkObject.SpawnWithOwnership(ownerClientId);
        retval.transform.position = pos;

        return retval.GetComponent<IPlayableCharacter>();
    }

    public IStatus ObjectCharacter { get; set; }

    public IStatus InstantiateStatus()
    {
        if (!IsHost) return null;
        
        GameObject go = ResourceManager.Instance.
            ResourceConnector.ImportGameObject("UNetOC");
        
        UNetObjectCharacter retval = go.GetComponent<UNetObjectCharacter>();
        retval.GetComponent<NetworkObject>().
            SpawnWithOwnership(NetworkManager.ServerClientId);

        retval.SetTargetCharacter("Baron");

        return retval;
    }

    public void StartMatch()
    {
        ResetMatchState();
        StartMatchClientRpc();
    }

    [ClientRpc]
    public void StartMatchClientRpc()
    {

        MatchGenerator generator = GameObject.FindWithTag
            ("MatchGenerator").GetComponent<MatchGenerator>();

        generator.SetMatchInfo(GameManager.Instance.MatchInfo);
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
        if (player == null || player.TurnState.TeamIdx < 0) return;
        if (Players.Contains(player)) return;

        while (Players.Count <= player.TurnState.TeamIdx)
        {
            Players.Add(null);
        }

        Players[player.TurnState.TeamIdx] = player;
    }

    public bool GameProceed()
    {
        int alivePlayers = 0;

        for (int i = 0; i < Players.Count; i++)
        {
            if (Players[i] == null) continue;
            if (_deathPlayerIdx.Contains(i)) continue;
            alivePlayers++;
        }

        return alivePlayers > 1;
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
        if (!IsServer) return;
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
        if (_matchStarted)
        {
            return;
        }

        _readyPlayerCnt++;

        if (_readyPlayerCnt >= GameManager.Instance.MatchInfo.PlayerInfors.Count)
        {
            _matchStarted = true;
            GameStartClientRpc();
            TurnManager.Instance.System.SetGameProceedCondition(GameProceed);
            TurnManager.Instance.System.StartGame();
        }
    }

    [ClientRpc]
    public void GameStartClientRpc()
    {
        UNetMatchInfo matchInfo = GameManager.Instance.MatchInfo as UNetMatchInfo;
        int localPlayerIdx = matchInfo != null
            ? matchInfo.GetPlayerIdx(NetworkManager.Singleton.LocalClientId)
            : (NetManager.Instance?.System?.Id ?? -1);

        if (localPlayerIdx >= 0 && localPlayerIdx < Players.Count
            && Players[localPlayerIdx] != null)
        {
            Players[localPlayerIdx].SetController(new GUICharacterController());
        }

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
        _matchStarted = false;
        _readyPlayerCnt = 0;

        if (BoardManager.Instance.Map != null)
        {
            Destroy(BoardManager.Instance.Map.gameObject);
        }

        BoardManager.Instance.Map = null;

        foreach (var player in Players)
        {
            if (player == null)
            {
                continue;
            }

            if (player.Status.IsAlive())
            {
                IGUIFullScreen nowScreen = UIManager.Instance.NowDisplay;
                UIManager.Instance.OpenGUI<GUIResult>("Result").SetWinner(player.Status);
                SFXManager.Instance.PlayBGM("Win");
                nowScreen?.Close();
            }
            player.Dispose();
        }

        ResetMatchState();
        GameManager.Instance.Playground = new Playground();
        TurnManager.Instance.System = new TurnSystem();
    }

    private void ResetMatchState()
    {
        Players = new List<IPlayableCharacter>();
        _deathPlayerIdx = new HashSet<int>();
        _readyPlayerCnt = 0;
        _matchStarted = false;
    }

    public int CalcDamage(IStatus attacker, IStatus target)
    {
        return Mathf.Max(1, attacker.Atk - target.Dfs);
    }
    
}
