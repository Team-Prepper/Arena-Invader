using System;

public interface ICharacterController : IBoardPlayer {

    public int PlayerId { get; }

    public BasePlayer Target { get; }
    public IStatus Status { get; }

    public void SetMatch(ICharacterActionSelector selector);
    public void SetTargetCharacter(string name, string characterCode, int idx);

    GUIDice RollDice(Action<int> value);
    public void OpenShop(Action callback);
    GUIOpenInventory OpenInventory();
    GUISelectMovePawn SelectMovePawn(int value);
    
    public void MovePawn(int pawnId, int amount);

}