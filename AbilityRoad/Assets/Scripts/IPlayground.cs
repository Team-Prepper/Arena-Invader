using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayground
{
    public IList<Character> Players { get; }
    public Map Map { get; set; }
    public int Turn { get; }

    public void AddPlayer(Character player);
    public void TurnStart();
    public void TurnEnd();
    public int CalcDamage(Character attacker, Character target);

}
