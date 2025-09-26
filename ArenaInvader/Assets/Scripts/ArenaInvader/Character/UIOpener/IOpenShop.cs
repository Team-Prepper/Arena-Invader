using System;

public interface IOpenShop
{

    public void Initial(IPlayableCharacter cc);
    public GUIShop OpenShop(Action callback);
}