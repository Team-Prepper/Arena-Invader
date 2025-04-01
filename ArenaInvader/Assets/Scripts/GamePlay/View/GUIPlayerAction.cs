using UnityEngine;
using UnityEngine.UI;

public class GUIPlayerAction : GUINetworkPopUp<int> {

    [SerializeField] private Button _useItemButton;
    [SerializeField] private Button _rollDiceButton;

    private ICharacterActionSelector _selector;

    public void PlayerTurnStart(ICharacterActionSelector selector) {
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
