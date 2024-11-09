using System.Collections;
using System.Collections.Generic;
using EHTool.UIKit;
using UnityEngine;

public class OpenShop : IArriveEvent
{
    public override int GetPriority() => 2;

    public override int GetValue(Character attacker, Character defender)
    {
        return 0;
    }

    public override void AddAbility(Pawn target, CallbackMethod callback)
    {
        target.GetOwner().EnterShop(callback);
    }
}

