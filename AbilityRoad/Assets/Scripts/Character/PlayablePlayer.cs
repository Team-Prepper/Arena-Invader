using EHTool.UIKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayablePlayer : BasePlayer {

    protected override void RollDice()
    {
        _chance--;

        UIManager.Instance.OpenGUI<GUIDice>("Dice").SetCallback((value) => {
            UIManager.Instance.OpenGUI<GUISelectMovePawn>("SelectMovePawn").SetPlayer(this, value);
        });

    }
}