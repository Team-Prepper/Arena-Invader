using System;

public interface ICharacterController {

    public BasePlayer Target { get; }

    public void SetMatch(ICharacterActionSelector selector);
    public void SetTargetCharacter(string name, string characterCode, int idx);

    public void StartTurn();
    public void EndTurn();

    GUIDice RollDice(Action<int> value);
    public void AddChance();
    void GetExtraDicePoint(int point);

    GUIOpenInventory OpenInventory();

    GUISelectMovePawn SelectMovePawn(int value);
    public void MovePawn(int pawnId, int amount);

    public void OpenShop(Action callback);

    public void AbilityChange(string abilityType, string amount);
}