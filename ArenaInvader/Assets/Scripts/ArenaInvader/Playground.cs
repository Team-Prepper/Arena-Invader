using System.Collections.Generic;
using UnityEngine;
using EasyH;
using EasyH.Unity.UI;
using EasyH.Gaming.TurnBased;

public class Playground : IPlayground
{

    private ISet<int> _deathPlayerIdx;

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
            AssetOpener.ImportComponent<PlayableCharacter>("LocalCC");
        retval.transform.position = pos;

        return retval;
    }

    public IStatus InstantiateStatus()
    {
        LocalObjectCharacter retval =
            AssetOpener.ImportComponent<LocalObjectCharacter>("LocalOC");

        retval.SetTargetCharacter("Baron");

        return retval;
    }

    public void StartMatch()
    {

        MatchGenerator generator = GameObject.FindWithTag
            ("MatchGenerator").GetComponent<MatchGenerator>();
        generator.SetMatchInfor(GameManager.Instance.MatchInfor);
        generator.Generate();

        BoardManager.Instance.Map =
            AssetOpener.ImportComponent<GameMap>
                (GameManager.Instance.MatchInfor.MapName);

    }

    public void MatchLoadComplete()
    {
        UIManager.Instance.OpenGUI<GUIPlayground>(
            "Playground").Generate();

        GUICharacterController _guiActionSelector =
            new GUICharacterController();
        AICharacterController _aiActionSelector =
            new GameObject().AddComponent<AICharacterController>();

        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].SetController(
                GameManager.Instance.MatchInfor.PlayerInfors[i].
                CharacterCode.Equals("Player_AI") ?
                _aiActionSelector : _guiActionSelector);

        }
        
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
}