using System;

public interface ICharacterController : IBoardPlayer {

    public int PlayerId { get; }

    public BasePlayer Target { get; }
    public IStatus Status { get; }
    public IInventory Inventory { get; }

    public void SetMatch(ICharacterActionSelector selector);
    public void SetTargetCharacter(string name, string characterCode, int idx);

    GUIDice OpenRollDice(Action<int> value);
    public void OpenShop(Action callback);
    GUIOpenInventory OpenInventory();
    GUISelectMovePawn OpenSelectMovePawn(int value);
    GUIBattle OpenBattle(int target, Action callback);
    
    public void MovePawn(int pawnId, int amount);

}