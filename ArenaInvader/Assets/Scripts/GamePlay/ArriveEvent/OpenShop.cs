using System;

public class OpenShop : IArriveEvent
{
    public override int GetPriority() => 2;

    public override int GetValue(IStatus attacker, IStatus defender)
    {
        return 0;
    }

    public override void AddAbility(GamePawn target, Action callback)
    {
        target.GetCharacterController().OpenShop(callback);
    }
    
}