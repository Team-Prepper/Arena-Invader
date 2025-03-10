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
        int attackTarget = SetTarget(target.GetCharacterController());

        target.GetCharacterController().OpenBattle(attackTarget, callback);
    }

    public int SetTarget(ICharacterController cc)
    {
        if (GameManager.Instance.Playground.ObjectCharacter != null) return -1;

        foreach (var player in GameManager.Instance.Playground.Players)
        {
            if (player == cc) continue;
            if (!player.Status.IsAlive()) continue;

            return player.PlayerId;
        }

        return -1;
    }

}