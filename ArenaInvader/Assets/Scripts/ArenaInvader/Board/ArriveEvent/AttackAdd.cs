using System;
using UnityEngine;

public class AttackAdd : ArriveEventBase {

    [SerializeField] int _attackAmount = 0;

    public override int GetPriority() => 1;
    public override int GetValue(IStatus attacker, IStatus defender)
    {
        return _attackAmount;
    }

    public override void AddAbility(GamePawn target, Action callback)
    {
        target.AddAttack(_attackAmount);
        callback?.Invoke();
    }
}