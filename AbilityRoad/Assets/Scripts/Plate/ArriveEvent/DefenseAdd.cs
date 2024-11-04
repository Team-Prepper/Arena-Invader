using System;
using UnityEngine;

public class DefenceAdd : ArriveEvent {

    [SerializeField] int _defenceAmount = 0;

    public override void AddAbility(Player target, int amount)
    {
        target.AddDefence(_defenceAmount * amount);
    }
}