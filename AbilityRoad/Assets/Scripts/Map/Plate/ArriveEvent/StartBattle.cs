using EHTool.UIKit;
using JetBrains.Annotations;
using System;

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

    public override void AddAbility(Pawn target, Action callback)
    {
        ObjectCharacter _object = GameManager.Instance.Playground.Map.GetObject();
        if (_object != null) {
            UIManager.Instance.OpenGUI<GUIRaid>("Raid").StartBattle(target.GetOwner(), _object, callback);
            return;
        }

        Character attackTarget = SetTarget(target.GetOwner());

        if (attackTarget == null) {
            callback?.Invoke();
            return;
        }

        UIManager.Instance.OpenGUI<GUIBattle>("Battle").
            StartBattle(target.GetOwner(), attackTarget, callback);
    }

    public Character SetTarget(Character attacker)
    {

        foreach (var player in GameManager.Instance.Playground.Players)
        {
            if (player.Target == attacker) continue;
            if (!player.Target.IsAlive()) continue;

            return player.Target;
        }

        return null;
    }

}