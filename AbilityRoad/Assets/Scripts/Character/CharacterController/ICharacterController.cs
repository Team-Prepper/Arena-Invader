using System;

public interface ICharacterController {

    public BasePlayer Target { get; }

    public void SetTargetCharacter(string characterCode, string name, ICharacterActionSelector selector);

    public void StartTurn();
    public void EndTurn();

    GUIDice RollDice(Action<int> value);
    public void AddChance();
    void GetExtraDicePoint(int point);

    GUIOpenInventory OpenInventory(Action<int> value);

    GUISelectMovePawn SelectMovePawn(int value);
    public void MovePawn(int pawnId, int amount);

    public void EnterShop(Action callback);

    public void AbilityChange(string abilityType, string amount);
}