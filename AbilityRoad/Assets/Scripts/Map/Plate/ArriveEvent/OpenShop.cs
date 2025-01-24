using System;

public class OpenShop : IArriveEvent
{
    public override int GetPriority() => 2;

    public override int GetValue(Character attacker, Character defender)
    {
        return 0;
    }

    public override void AddAbility(Pawn target, Action callback)
    {
        target.GetOwner().EnterShop(callback);
    }
}