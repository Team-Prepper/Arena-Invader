using System;

public interface IOpenBattle
{

    public void Initial(IPlayableCharacter cc);
    public GUIBattle OpenBattle(int attacker, int target, Action callback);
}