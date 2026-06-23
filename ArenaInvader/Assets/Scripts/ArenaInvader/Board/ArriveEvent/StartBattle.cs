using System;

public class StartBattle : ArriveEventBase {

    public override int GetPriority() => 2;

    public override int GetValue(IStatus attacker, IStatus defender)
    {
        if (attacker == null || defender == null)
        {
            return 0;
        }

        IStatus _object = GameManager.Instance.Playground.ObjectCharacter;

        if (_object != null)
        {
            defender = _object;
        }

        int combatValue = GameManager.Instance.Playground.CalcDamage(attacker, defender);

        return combatValue;
    }

    public override void AddAbility(GamePawn target, Action callback)
    {
        if (target == null || target.GetCC() == null)
        {
            callback?.Invoke();
            return;
        }

        int attackTarget = SetTarget(target.GetCC());
        if (attackTarget < 0)
        {
            callback?.Invoke();
            return;
        }

        target.GetCC().OpenBattle(attackTarget, callback);
    }

    public int SetTarget(IPlayableCharacter cc)
    {
        if (cc == null) return -1;
        if (GameManager.Instance.Playground.ObjectCharacter != null) return -1;

        foreach (var player in GameManager.Instance.Playground.Players)
        {
            if (player == cc) continue;
            if (!player.Status.IsAlive()) continue;

            return player.TurnState.TeamIdx;
        }

        return -1;
    }

}
