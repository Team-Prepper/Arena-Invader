using System;
using UnityEngine;

public class HealAdd : ArriveEventBase {

    [SerializeField] int _healAmount = 0;

    public override int GetPriority() => 1;

    public override int GetValue(IStatus attacker, IStatus defender)
    {
        return _healAmount;
    }

    public override void AddAbility(GamePawn target, Action callback)
    {
        if (target == null)
        {
            callback?.Invoke();
            return;
        }

        target.AddHealth(_healAmount);
        callback?.Invoke();
    }
}
