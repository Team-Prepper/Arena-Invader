using System;
using UnityEngine;

public class HealAdd : IArriveEvent {

    [SerializeField] int _healAmount = 0;
    
    public override int GetValue()
    {
        return _healAmount;
    }

    public override void AddAbility(Pawn target, CallbackMethod callback)
    {
        target.AddHealth(_healAmount);
        callback?.Invoke();
    }
}