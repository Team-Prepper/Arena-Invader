using System;
using UnityEngine;

public class MoneyAdd : IArriveEvent {
    public override int GetPriority() => 0;

    [SerializeField] int _moneyAmount = 0;

    public override int GetValue(GamePawn attacker, GamePawn defender)
    {
        return 0;
    }

    public override void AddAbility(GamePawn target, Action callback)
    {
        target.AddMoney(_moneyAmount);
        callback?.Invoke();
    }
}

