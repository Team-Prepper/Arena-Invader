using EHTool.UIKit;
using System;
using UnityEngine;

public class StartBattle : IArriveEvent {
    
    public override int GetValue(Character attacker, Character defender)
    {
        int combatValue = GameManager.Instance.Playground.CalcDamage(attacker, defender);
        return combatValue;
    }

    public override void AddAbility(Pawn target, CallbackMethod callback)
    {
        UIManager.Instance.OpenGUI<GUIBattle>("Battle").StartBattle(target.GetOwner(), callback);
    }
}