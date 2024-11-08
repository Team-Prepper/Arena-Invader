using System;
using UnityEngine;

public class AttackAdd : IArriveEvent {

    [SerializeField] int _attackAmount = 0;
    
    public override int GetValue()
    {
        return _attackAmount;
    }

    public override void AddAbility(Pawn target, CallbackMethod callback)
    {
        target.AddAttack(_attackAmount);
        callback?.Invoke();
    }
}