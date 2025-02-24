using UnityEngine;
using UnityEngine.UI;

public class GUIPlayerAction : GUINetworkPopUp<int> {

    [SerializeField] Button _useItemButton;
    [SerializeField] Button _rollDiceButton;

    ICharacterActionSelector _selector;

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

    /*
    public void PlayerTurnStart(BasePlayer player)
    {
        _rollDiceButton.onClick.RemoveAllListeners();
        _rollDiceButton.onClick.AddListener(() => player.RollDice());
        
        _useItemButton.onClick.RemoveAllListeners();
        _useItemButton.onClick.AddListener(() => player.OpenInventory(Close));
    }*/
    
}
