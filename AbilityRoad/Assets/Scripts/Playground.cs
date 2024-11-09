using EHTool.UIKit;
using System.Collections.Generic;
using UnityEngine;

public class Playground : IPlayground {

    public IList<BasePlayer> Players { get; private set; }
    public BasePlayer NowPlayer => Players[_turnIdx];
    public Map Map { get; set; }
    public int Turn { get; private set; }

    IList<int> _deathPlayerIdx;
    int _turnIdx;

    string _matchDiceCode = "DartDice";

    public Playground()
    {
        Players = new List<BasePlayer>();
        _turnIdx = 0;
        Turn = 0;
    }

    public void AddPlayer(BasePlayer player)
    {
        if (Players.Contains(player)) return;
        Players.Add(player);
    }

    public void PlayerDeath(BasePlayer player)
    {
        for (int i = 0; i < Players.Count; i++)
        {
            if (Players[i] != player) continue;
            _deathPlayerIdx.Add(i);
            break;
        }

        if (Players.Count - _deathPlayerIdx.Count < 2)
        {
            GameEnd();
        }
    }

    void GameEnd()
    {

    }

    public void TurnStart()
    {
        GUITurnStart turnStartCall = UIManager.Instance.OpenGUI<GUITurnStart>("TurnStart");

        turnStartCall.SetWaitForCallback(() => {
            Players[_turnIdx].StartTurn();
            turnStartCall.Close();
        });
    }

    public void TurnEnd()
    {

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