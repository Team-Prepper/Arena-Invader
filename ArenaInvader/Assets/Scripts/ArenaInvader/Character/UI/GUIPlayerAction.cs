using UnityEngine;
using UnityEngine.UI;

public class GUIPlayerAction : GUINetworkPopUp<int> {

    [SerializeField] private Button _useItemButton;
    [SerializeField] private Button _rollDiceButton;

    private ICharacterController _selector;

    public void PlayerTurnStart(ICharacterController selector) {
        _selector = selector;
        RefreshControlState();
    }

    public void SetIsNotControlled()
    {
        base.SetIsNotControlled();
        RefreshControlState();
    }

    public void RollDice() {
        if (!IsControlled) return;
        _selector.RollDice();
        Close();
    }

    public void OpenInventory() {
        if (!IsControlled) return;
        _selector.Inventory();
    }

    private void RefreshControlState()
    {
        if (_useItemButton != null)
        {
            _useItemButton.interactable = IsControlled;
        }

        if (_rollDiceButton != null)
        {
            _rollDiceButton.interactable = IsControlled;
        }
    }
    
}
