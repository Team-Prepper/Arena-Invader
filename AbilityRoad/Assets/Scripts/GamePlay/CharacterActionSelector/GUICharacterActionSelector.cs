using EHTool.UIKit;
using System;
using UnityEngine;

public class GUICharacterActionSelector : ICharacterActionSelector {

    ICharacterController _target;

    public void StartTurn(ICharacterController target)
    {
        _target = target;

        GUIPlayerAction action =
            UIManager.Instance.OpenGUI<GUIPlayerAction>("PlayerAction");
        
        action.PlayerTurnStart(this);

        Debug.Log("StartTurn");
    }

    public void RollDice()
    {
        GUIDice guiDice = _target.OpenRollDice((value) => {
            _target.OpenSelectMovePawn(value);
        });

    }

    public void Inventory()
    {
        GUIOpenInventory inventory = _target.OpenInventory();

    }

    public void Shop(GUIShop shop) {
        
    }

}