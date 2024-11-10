using EHTool.LangKit;
using EHTool.UIKit;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Playground : IPlayground {

    public IList<BasePlayer> Players { get; private set; }
    public BasePlayer NowPlayer => Players[_turnIdx];
    public Map Map { get; set; }
    public int Turn { get; private set; }

    ISet<int> _deathPlayerIdx;
    int _turnIdx;

    string _matchDiceCode = "DartDice";

    public Playground()
    {
        Players = new List<BasePlayer>();
        _deathPlayerIdx = new HashSet<int>();
        _turnIdx = 0;
        Turn = 0;
    }

    public void AddPlayer(BasePlayer player)
    {
        if (Players.Contains(player)) return;
        Players.Add(player);
    }

    public bool IsGameEnd()
    {
        return Players.Count - _deathPlayerIdx.Count < 2;
    }

    public void PlayerDeath(BasePlayer player)
    {
        for (int i = 0; i < Players.Count; i++)
        {
            if (Players[i] != player) continue;
            _deathPlayerIdx.Add(i);
            break;
        }

        if (IsGameEnd())
        {
            GameEnd();
        }
    }

    void GameEnd()
    {
        GameManager.Instance.Playground = new Playground();
        Object.Destroy(Map.gameObject);
        Map = null;
        foreach (var player in Players)
        {
            if (player.IsAlive())
            {
                IGUIFullScreen nowScreen = UIManager.Instance.NowDisplay;
                UIManager.Instance.OpenGUI<GUIResult>("Result").SetWinner(player);
                SFXManager.Instance.PlayBGM("Win");
                nowScreen.Close();
            }
            Object.Destroy(player.gameObject);
        }
    }

    public void TurnStart()
    {
        GUITurnStart turnStartCall = UIManager.Instance.OpenGUI<GUITurnStart>("TurnStart");

        turnStartCall.SetWaitForCallback(string.Format(LangManager.Instance.GetStringByKey("msg_XTurn"), Players[_turnIdx].GetName()), () => {
            Players[_turnIdx].StartTurn();
            turnStartCall.Close();
        });
    }

    public void TurnEnd()
    {
        if (IsGameEnd()) return;

        while (true)
        {
            _turnIdx = _turnIdx + 1;

            if (_turnIdx >= Players.Count)
            {
                Turn++;
                _turnIdx = 0;
                Map.StartNewTurn(Turn);
            }
            if (Players[_turnIdx].IsAlive()) break;
        }

        TurnStart();
    }

    public int CalcDamage(Character attacker, Character target)
    {
        return Mathf.Max(1, attacker.GetAttackValue() - target.GetDefenseValue());
    }
    public void SetMatchDice(string code)
    {
        _matchDiceCode = code;
    }

    public GUIDice GetMatchDice()
    {
        return UIManager.Instance.OpenGUI<GUIDice>(_matchDiceCode);
    }
}