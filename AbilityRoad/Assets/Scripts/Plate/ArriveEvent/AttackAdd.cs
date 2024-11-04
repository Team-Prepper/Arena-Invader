using System;
using UnityEngine;

public class AttackAdd : IArriveEvent {

    [SerializeField] int _attackAmount = 0;

    public override void AddAbility(Pawn target, int amount, CallbackMethod callback)
    {
        target.GetOwner().AddAttack(_attackAmount * amount);
        callback?.Invoke();
    }
}