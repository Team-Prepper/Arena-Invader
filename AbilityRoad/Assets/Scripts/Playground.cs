using System.Collections.Generic;
using UnityEngine;

public class Playground : IPlayground {
    
    IList<Player> _players;
    int _turnIdx;

    public Map Map { get; set; }
    public int Turn { get; private set; }

    public Playground() { 
        _players = new List<Player>();
        _turnIdx = 0;
        Turn = 0;
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
        _turnIdx = _turnIdx + 1;

        if (_turnIdx >= _players.Count) {
            Turn++;
            _turnIdx = 0;
        }

        TurnStart();
    }

    public void Battle(Player attacker, CallbackMethod callback) { 
        
    }

}