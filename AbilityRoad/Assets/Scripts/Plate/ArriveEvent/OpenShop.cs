using System.Collections;
using System.Collections.Generic;
using EHTool.UIKit;
using UnityEngine;

public class OpenShop : IArriveEvent
{
    public override int GetValue(Character attacker, Character defender)
    {
        return 0;
    }

    public override void AddAbility(Pawn target, CallbackMethod callback)
    {
        UIManager.Instance.OpenGUI<GUIShop>("Shop").EnterShop(target.GetOwner(), callback); // add to xml "shop"
    }
}

