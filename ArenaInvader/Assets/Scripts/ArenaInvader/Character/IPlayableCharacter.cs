using System;
using EasyH.Gaming.TurnBased;

public interface IPlayableCharacter
{

    public IStatus Status { get; }
    public IInventory Inventory { get; }
    public IMemberState TurnState { get; }
    public IPawnOwner PawnOwner { get; }

    public void GetExtraDicePoint(int dicePoint);
    public void SetController(ICharacterController selector);
    
    public void AddChance();
    public void EndTurn();
    public void Dispose();

    public GUIDice OpenRollDice(Action<int> value);
    public void OpenShop(Action callback);
    public GUIInventory OpenInventory();
    public GUISelectMovePawn OpenSelectMovePawn(int value);
    public GUIBattle OpenBattle(int target, Action callback);
    public void ShowUseItem(string itemCode);
}