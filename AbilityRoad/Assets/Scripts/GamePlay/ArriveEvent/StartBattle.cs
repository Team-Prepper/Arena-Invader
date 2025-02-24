using EHTool.UIKit;
using System;

public class StartBattle : IArriveEvent {

    public override int GetPriority() => 2;

    public override int GetValue(GamePawn attacker, GamePawn defender)
    {
        /*Character _object = GameManager.Instance.Playground.Map.GetObject();
        if (_object == null)
        {
            _object = defender;
        }*/
        
        //int combatValue = GameManager.Instance.Playground.CalcDamage(attacker, defender);
        //return combatValue;
        return 1;
    }

    public override void AddAbility(GamePawn target, Action callback)
    {
        /*
        ObjectCharacter _object = GameManager.Instance.Playground.Map.GetObject();
        if (_object != null) {
            UIManager.Instance.OpenGUI<GUIRaid>("Raid").StartBattle(target.GetOwner(), _object, callback);
            return;
        }
            */

        ICharacterController attackTarget = SetTarget(target.GetCharacterController());

        if (attackTarget == null) {
            callback?.Invoke();
            return;
        }

        UIManager.Instance.OpenGUI<GUIBattle>("Battle").
            StartBattle(target.GetCharacterController().PlayerId, attackTarget.PlayerId, callback);
    }

    public ICharacterController SetTarget(ICharacterController cc)
    {

        foreach (var player in GameManager.Instance.Playground.Players)
        {
            if (player == cc) continue;
            if (!player.Status.IsAlive()) continue;

            return player;
        }

        return null;
    }

}