using System.Collections.Generic;
using UnityEngine;
using EasyH.Unity;
using EasyH.Unity.UI;
using EasyH.Gaming.TurnBased;

public class Playground : IPlayground
{

    private ISet<int> _deathPlayerIdx;
    private GUICharacterController _guiController;
    private GameObject _aiControllerHost;
    private AICharacterController _aiController;

    public IList<IPlayableCharacter> Players { get; private set; }

    public IPlayableCharacter NowPlayer
    {
        get
        {
            if (Players == null || TurnManager.Instance.System.
                ActiveTeamIdx >= Players.Count) return null;
            return Players[TurnManager.Instance.System.ActiveTeamIdx];
        }

    }

    public IStatus ObjectCharacter { get; set; }

    public Playground()
    {
        Players = new List<IPlayableCharacter>();

        _deathPlayerIdx = new HashSet<int>();
    }

    public bool GameProceed()
    {
        return Players.Count - _deathPlayerIdx.Count > 1;
    }

    public IPlayableCharacter InstantiateCC(Vector3 pos)
    {
        PlayableCharacter retval =
            ResourceManager.Instance.ResourceConnector.
                ImportComponent<PlayableCharacter>("LocalCC");
        retval.transform.position = pos;

        return retval;
    }

    public IStatus InstantiateStatus()
    {
        LocalObjectCharacter retval =
            ResourceManager.Instance.ResourceConnector.
                ImportComponent<LocalObjectCharacter>("LocalOC");

        retval.SetTargetCharacter("Baron");

        return retval;
    }

    public void StartMatch()
    {

        MatchGenerator generator = GameObject.FindWithTag
            ("MatchGenerator").GetComponent<MatchGenerator>();

        generator.EnsureDefaultMatchInfo();

        generator.SetMatchInfo(GameManager.Instance.MatchInfo);
        generator.Generate();

        BoardManager.Instance.Map =
            ResourceManager.Instance.ResourceConnector.
                ImportComponent<GameMap>(
                    GameManager.Instance.MatchInfo.MapName);

    }

    public void MatchLoadComplete()
    {
        EnsureControllers();

        UIManager.Instance.OpenGUI<GUIPlayground>(
            "Playground").Generate();
        RefreshPlayerControllers();
        
        TurnManager.Instance.System.
            SetGameProceedCondition(GameProceed);
        TurnManager.Instance.System.StartGame();

    }

    public void AddPlayer(IPlayableCharacter player)
    {
        if (Players.Contains(player)) return;
        Players.Add(player);
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
        if (GameProceed()) return;
        GameEnd();
    }

    void GameEnd()
    {
        DisposeControllers();
        Object.Destroy(BoardManager.Instance.Map.gameObject);

        BoardManager.Instance.Map = null;

        foreach (var player in Players)
        {
            if (player.Status.IsAlive())
            {
                IGUIFullScreen nowScreen = UIManager.Instance.NowDisplay;
                UIManager.Instance.OpenGUI<GUIResult>("Result").SetWinner(player.Status);
                //SFXManager.Instance.PlayBGM("Win");
                nowScreen.Close();
            }
            player.Dispose();
        }
        GameManager.Instance.Playground = new Playground();
    }

    public int CalcDamage(IStatus attacker, IStatus target)
    {
        return Mathf.Max(1, attacker.Atk - target.Dfs);
    }

    public void RefreshPlayerControllers()
    {
        EnsureControllers();

        for (int i = 0; i < Players.Count; i++)
        {
            ApplyController(i);
        }
    }

    public void SetPlayerControlMode(int playerIdx, bool isAI)
    {
        if (GameManager.Instance?.MatchInfo == null)
        {
            return;
        }

        if (playerIdx < 0 || playerIdx >= GameManager.Instance.MatchInfo.PlayerInfors.Count)
        {
            return;
        }

        GameManager.Instance.MatchInfo.SetPlayerIsAI(playerIdx, isAI);

        if (playerIdx >= Players.Count)
        {
            return;
        }

        ApplyController(playerIdx);
    }

    private void ApplyController(int playerIdx)
    {
        if (playerIdx < 0 || playerIdx >= Players.Count)
        {
            return;
        }

        PlayerInfor playerInfo = GameManager.Instance.MatchInfo.PlayerInfors[playerIdx];
        Players[playerIdx].SetController(playerInfo.IsAI ? _aiController : _guiController);
    }

    private void EnsureControllers()
    {
        if (_guiController == null)
        {
            _guiController = new GUICharacterController();
        }

        if (_aiController != null)
        {
            return;
        }

        _aiControllerHost = new GameObject("AICharacterController");
        _aiController = _aiControllerHost.AddComponent<AICharacterController>();
    }

    private void DisposeControllers()
    {
        _guiController = null;
        _aiController = null;

        if (_aiControllerHost == null)
        {
            return;
        }

        Object.Destroy(_aiControllerHost);
        _aiControllerHost = null;
    }
}
