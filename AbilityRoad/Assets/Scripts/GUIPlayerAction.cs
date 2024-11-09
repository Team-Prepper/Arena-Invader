using System.Collections;
using System.Collections.Generic;
using EHTool.UIKit;
using UnityEngine;
using UnityEngine.UI;

public class GUIPlayerAction : GUIPopUp
{
    [SerializeField] Button _useItemButton;
    [SerializeField] Button _rollDiceButton;

    public void PlayerTurnStart(BasePlayer player)
    {
        _rollDiceButton.onClick.RemoveAllListeners();
        _rollDiceButton.onClick.AddListener(() => player.RollDice());
        
        _useItemButton.onClick.RemoveAllListeners();
        _useItemButton.onClick.AddListener(() => player.OpenInventory(Close));
    }
    
}
