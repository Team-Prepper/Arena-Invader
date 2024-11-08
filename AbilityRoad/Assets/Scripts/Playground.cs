using System.Collections.Generic;
using UnityEngine;

public class Playground : IPlayground {

    public IList<Character> Players { get; private set; }
    public Map Map { get; set; }
    public int Turn { get; private set; }

    int _turnIdx;

    public Playground() { 
        Players = new List<Character>();
        _turnIdx = 0;
        Turn = 0;
    }

    public void AddPlayer(Character player)
    {
        if (Players.Contains(player)) return;
        Players.Add(player);
    }

    public void TurnStart()
    {
        Players[_turnIdx].StartTurn();
    }

    public void TurnEnd()
    {
        _turnIdx = _turnIdx + 1;

        if (_turnIdx >= Players.Count) {
            Turn++;
            _turnIdx = 0;
        }

        TurnStart();
    }
    
    public int CalcDamage(Character attacker, Character target)
    {
        return Mathf.Max(1, attacker.GetAttackValue() - target.GetDefenseValue());
    }

}