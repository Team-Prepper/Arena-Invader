
using EasyH.Unity.UI;

public class GUICharacterController : ICharacterController {

    private IPlayableCharacter _target;

    public void StartTurn(IPlayableCharacter target)
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