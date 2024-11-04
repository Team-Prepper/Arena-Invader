using System;
using UnityEngine;

public class AttackAdd : ArriveEvent {

    [SerializeField] int _attackAmount = 0;

    public override void AddAbility(Player target, int amount)
    {
        target.AddAttack(_attackAmount * amount);
    }
}