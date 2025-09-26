using System;

public interface IOpenDice
{
    public void Initial(IPlayableCharacter cc);
    public GUIDice OpenDice(Action<int> callback);
}