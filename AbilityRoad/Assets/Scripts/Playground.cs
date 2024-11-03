using System.Collections.Generic;
using UnityEngine;

public class Playground {
    
    IList<Player> _players;
    int _turnIdx;

    public Map Map { get; internal set; }

    public Playground() { 
        _players = new List<Player>();
        _turnIdx = 0;
    }

    public void AddPlayer(Player player)
    {
        if (_players.Contains(player)) return;
        _players.Add(player);
    }

    public void TurnStart()
    {
        _players[_turnIdx].StartTurn();
    }

    public void TurnEnd()
    {
        _turnIdx = (_turnIdx + 1) % _players.Count;
        TurnStart();
    }

}