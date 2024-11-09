using EHTool.UIKit;
using System;
using System.Xml.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class StartBattle : IArriveEvent {

    public override int GetPriority() => 2;
    public override int GetValue(Character attacker, Character defender)
    {
        Character _object = GameManager.Instance.Playground.Map.GetObject();
        if (_object == null)
        {
            _object = defender;
        }
        int combatValue = GameManager.Instance.Playground.CalcDamage(attacker, defender);
        return combatValue;
    }

    public override void AddAbility(Pawn target, CallbackMethod callback)
    {
        ObjectCharacter _object = GameManager.Instance.Playground.Map.GetObject();
        if (_object != null) {
            UIManager.Instance.OpenGUI<GUIRaid>("Raid").StartRaid(target.GetOwner(), _object, callback);
            return;
        }
        UIManager.Instance.OpenGUI<GUIBattle>("Battle").StartBattle(target.GetOwner(), callback);
    }

}