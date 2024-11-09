using System;
using UnityEngine;

public class DefenceAdd : IArriveEvent {

    [SerializeField] int _defenceAmount = 0;

    public override int GetPriority() => 1;
    public override int GetValue(Character attacker, Character defender)
    {
        return _defenceAmount;
    }

    public override void AddAbility(Pawn target, CallbackMethod callback)
    {
        target.AddDefence(_defenceAmount);
        callback?.Invoke();
    }
}