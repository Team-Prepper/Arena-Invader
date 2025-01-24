using EHTool.UIKit;
using UnityEngine;
using UnityEngine.UI;

public class GUIPlayerAction : GUIPopUp
{
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
        //_selector.Target.OpenInventory();
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
