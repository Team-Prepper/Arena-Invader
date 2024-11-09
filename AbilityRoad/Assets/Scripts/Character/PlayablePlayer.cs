using EHTool.UIKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayablePlayer : BasePlayer {
    
    public override void StartTurn()
    {
        base.StartTurn();
        UIManager.Instance.OpenGUI<GUIPlayerAction>("PlayerAction").PlayerTurnStart(this);
    }

    public override void RollDice()
    {
        _chance--;

        GameManager.Instance.Playground.GetMatchDice().SetCallback((value) => {
            UIManager.Instance.OpenGUI<GUISelectMovePawn>("SelectMovePawn").SetPlayer(this, value + extraDicePoint);
            extraDicePoint = 0;
        });
        
    }
    
    public override void EnterShop(CallbackMethod callback)
    {
        UIManager.Instance.OpenGUI<GUIShop>("Shop").EnterShop(this, callback); 
    }
    
}