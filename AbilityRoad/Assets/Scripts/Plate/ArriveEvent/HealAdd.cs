using System;
using UnityEngine;

public class HealAdd : ArriveEvent {

    [SerializeField] int _healAmount = 0;

    public override void AddAbility(Player target, int amount = 1)
    {
        target.AddHeal(_healAmount * amount);
    }
}