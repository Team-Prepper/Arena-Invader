using System;
using UnityEngine;

public class MoneyAdd : ArriveEventBase {
    public override int GetPriority() => 0;

    [SerializeField] int _moneyAmount = 0;

    public override int GetValue(IStatus attacker, IStatus defender)
    {
        return 0;
    }

    public override void AddAbility(GamePawn target, Action callback)
    {
        if (target == null)
        {
            callback?.Invoke();
            return;
        }

        target.AddMoney(_moneyAmount);
        callback?.Invoke();
    }
}
