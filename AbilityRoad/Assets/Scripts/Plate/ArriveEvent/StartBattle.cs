using System;
using UnityEngine;

public class StartBattle : ArriveEvent {

    [SerializeField] int _healAmount = 0;

    public override void AddAbility(Player target, int amount)
    {
        target.AddHeal(_healAmount);
    }
}