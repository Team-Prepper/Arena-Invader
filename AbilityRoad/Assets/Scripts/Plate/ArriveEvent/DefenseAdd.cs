using System;
using UnityEngine;

public class DefenceAdd : IArriveEvent {

    [SerializeField] int _defenceAmount = 0;
    
    public override int GetValue()
    {
        return _defenceAmount;
    }

    public override void AddAbility(Pawn target, CallbackMethod callback)
    {
        target.AddDefence(_defenceAmount);
        callback?.Invoke();
    }
}