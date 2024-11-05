using System;
using UnityEngine;

public class HealAdd : IArriveEvent {

    [SerializeField] int _healAmount = 0;

    public override void AddAbility(Pawn target, CallbackMethod callback)
    {
        target.AddHeal(_healAmount);
        callback?.Invoke();
    }
}