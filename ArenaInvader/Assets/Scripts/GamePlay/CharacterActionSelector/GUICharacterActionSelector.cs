using EHTool.UIKit;

public class GUICharacterActionSelector : ICharacterActionSelector {

    private ICharacterController _target;

    public void StartTurn(ICharacterController target)
    {
        _target = target;

        GUIPlayerAction action =
            UIManager.Instance.OpenGUI<GUIPlayerAction>("PlayerAction");
        
        action.PlayerTurnStart(this);
    }

    public void RollDice()
    {
        GUIDice guiDice = _target.OpenRollDice((value) => {
            _target.OpenSelectMovePawn(value);
        });

    }

    public void Inventory()
    {
        GUIInventory inventory = _target.OpenInventory();

    }

    public void Shop(GUIShop shop) {
        
    }

}