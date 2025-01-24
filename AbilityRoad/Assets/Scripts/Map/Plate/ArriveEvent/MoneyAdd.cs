using System;
using UnityEngine;

public class MoneyAdd : IArriveEvent {
    public override int GetPriority() => 0;

    [SerializeField] int _moneyAmount = 0;

    public override int GetValue(Character attacker, Character defender)
    {
        return 0;
    }

    public override void AddAbility(Pawn target, Action callback)
    {
        target.AddMoney(_moneyAmount);
        callback?.Invoke();
    }
}

