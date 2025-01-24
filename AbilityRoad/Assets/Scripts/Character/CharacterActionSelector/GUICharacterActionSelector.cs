using EHTool.UIKit;
using System;
using UnityEngine;

public class GUICharacterActionSelector : MonoBehaviour, ICharacterActionSelector {

    [SerializeField] ICharacterController _target;

    public void StartTurn(ICharacterController target)
    {
        _target = target;

        GUIPlayerAction action =
            UIManager.Instance.OpenGUI<GUIPlayerAction>("PlayerAction");
        
        action.PlayerTurnStart(this);

    }

    public void RollDice()
    {
        GUIDice guiDice = _target.RollDice((value) => {
            _target.SelectMovePawn(value);
        });

    }

    public void SelectItem(GUIShop shop, Action<int> callback)
    {

    }

}