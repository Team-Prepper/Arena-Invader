using System;
using UnityEngine;

public class HealAdd : IArriveEvent {

    [SerializeField] int _healAmount = 0;

    public override void AddAbility(Pawn target, int amount, CallbackMethod callback)
    {
        target.GetOwner().AddHeal(_healAmount * amount);
        callback?.Invoke();
    }
}