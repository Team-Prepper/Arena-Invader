using UnityEngine;
using UnityEngine.UI;

public class GUIPlayerAction : GUINetworkPopUp<int> {

    [SerializeField] private Button _useItemButton;
    [SerializeField] private Button _rollDiceButton;

    private ICharacterController _selector;

    public void PlayerTurnStart(ICharacterController selector) {
        _selector = selector;
    }

    public void RollDice() {
        _selector.RollDice();
        Close();
    }

    public void OpenInventory() {
        _selector.Inventory();
    }
    
}
