using System;
using UnityEngine;

public class DefenceAdd : IArriveEvent {

    [SerializeField] int _defenceAmount = 0;

    public override void AddAbility(Pawn target, int amount, CallbackMethod callback)
    {
        target.GetOwner().AddDefence(_defenceAmount * amount);
        callback?.Invoke();
    }
}