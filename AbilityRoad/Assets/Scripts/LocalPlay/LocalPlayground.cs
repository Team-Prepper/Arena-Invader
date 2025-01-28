using EHTool;
using EHTool.LangKit;
using EHTool.UIKit;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayground : IPlayground {

    public IList<ICharacterController> Players { get; private set; }

    public ICharacterController NowPlayer {
        get {
            if (Players == null || _turnIdx >= Players.Count) return null;
            return Players[_turnIdx];
        }
    
    } 

    public Map Map { get; set; }
    public int Turn { get; private set; }

    public MatchInfor MatchInfor { get; set; }

    ISet<int> _deathPlayerIdx;
    int _turnIdx;

    public ICharacterController InstantiateCC() {
        return AssetOpener.ImportComponent<LocalCharacterController>("LocalCC");
    }

    public LocalPlayground()
    {
        Players = new List<ICharacterController>();
        MatchInfor = new LocalMatchInfor(2, "Map/DefaultMap", "DartDice");

        _deathPlayerIdx = new HashSet<int>();
        _turnIdx = 0;
        Turn = 0;

    }

    public void AddPlayer(ICharacterController player)
    {
        if (Players.Contains(player)) return;
        Players.Add(player);
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

    void GameEnd()
    {
        GameManager.Instance.Playground = new LocalPlayground();
        Object.Destroy(Map.gameObject);
        Map = null;
        foreach (var player in Players)
        {
            if (player.Target.IsAlive())
            {
                IGUIFullScreen nowScreen = UIManager.Instance.NowDisplay;
                UIManager.Instance.OpenGUI<GUIResult>("Result").SetWinner(player.Target);
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
            string.Format(LangManager.Instance.GetStringByKey("msg_XTurn"), Players[_turnIdx].Target.GetName()), () => {
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
            if (Players[_turnIdx].Target.IsAlive()) break;
        }

        TurnStart();
    }

    public int CalcDamage(Character attacker, Character target)
    {
        return Mathf.Max(1, attacker.GetAttackValue() - target.GetDefenseValue());
    }

}