using EHTool.UIKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : Player
{
    protected override void RollDice()
    {
        _chance--;

        GUIDice guiDice = UIManager.Instance.OpenGUI<GUIDice>("Dice");
        guiDice.SetCallback((value) => {
            UIManager.Instance.OpenGUI<GUISelectMovePawn>("SelectMovePawn").SetPlayer(this, value);
        });
        
        guiDice.Roll();
    }

    private void Policy()
    {
        Pawn mostValuablePawn = null;
        int mostValuablePawnValue = 0;

        foreach (var pawn in _pawns)
        {
            /*Something to do*/
        }
    }

    private int ValueFunc(IPlate plate)
    {
        return plate.GetValue();
    }
}