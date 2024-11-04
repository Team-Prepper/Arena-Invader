using System;
using UnityEngine;

public class StartBattle : IArriveEvent {

    public override void AddAbility(Pawn target, int amount, CallbackMethod callback)
    {
        GameManager.Instance.Playground.Battle(target.GetOwner(), callback);
    }
}