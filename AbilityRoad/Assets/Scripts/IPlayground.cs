using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayground
{
    public Map Map { get; set; }
    public int Turn { get; }

    public void AddPlayer(Player player);
    public void TurnStart();
    public void TurnEnd();

    public void Battle(Player attacker, CallbackMethod callback);

}
