using EHTool.UIKit;
using System;
using UnityEngine;

public class StartBattle : IArriveEvent {
    
    public override int GetValue()
    {
        return 0;
    }

    public override void AddAbility(Pawn target, CallbackMethod callback)
    {
        UIManager.Instance.OpenGUI<GUIBattle>("Battle").StartBattle(target.GetOwner(), callback);
    }
}