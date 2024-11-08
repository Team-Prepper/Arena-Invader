using System.Collections.Generic;
using UnityEngine;

public class Playground : IPlayground {

    public IList<BasePlayer> Players { get; private set; }
    public Map Map { get; set; }
    public int Turn { get; private set; }

    int _turnIdx;

    public Playground() { 
        Players = new List<BasePlayer>();
        _turnIdx = 0;
        Turn = 0;
    }

    public void AddPlayer(BasePlayer player)
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

}