using System;
using UnityEngine;

public class DefenceAdd : ArriveEventBase {

    [SerializeField] int _defenceAmount = 0;

    public override int GetPriority() => 1;
    public override int GetValue(IStatus attacker, IStatus defender)
    {
        return _defenceAmount;
    }

    public override void AddAbility(GamePawn target, Action callback)
    {
        target.AddDefence(_defenceAmount);
        callback?.Invoke();
    }

}