using System;

public interface ICharacterController : IBoardPlayer {

    public int PlayerId { get; }

    public BasePlayer Target { get; }
    public IStatus Status { get; }
    public IInventory Inventory { get; }

    public void SetMatch(ICharacterActionSelector selector);
    public void SetTargetCharacter(string name, string characterCode, int idx);

    public GUIDice OpenRollDice(Action<int> value);
    public void OpenShop(Action callback);
    public GUIInventory OpenInventory();
    public GUISelectMovePawn OpenSelectMovePawn(int value);
    public GUIBattle OpenBattle(int target, Action callback);
    public void ShowUseItem(string itemCode);
    
    public void MovePawn(int pawnId, int amount);

}