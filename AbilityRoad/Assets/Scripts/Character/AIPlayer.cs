using EHTool.UIKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : BasePlayer
{
    protected override void RollDice()
    {
        _chance--;

        GUIDice guiDice = UIManager.Instance.OpenGUI<GUIDice>("Dice");
        guiDice.SetCallback((value) => {
            Policy(value).Move(value);
        });
        
        guiDice.Roll();
    }

    private Pawn Policy(int value)
    {
        Pawn mostValuablePawn = null;
        int mostValuablePawnValue = 0;

        foreach (var pawn in _pawns)
        {
            if(pawn.IsPiggyBacked()) continue;
            int currentPawnValue = pawn.MovePredict(value).GetValue();
            if (currentPawnValue > mostValuablePawnValue)
            {
                mostValuablePawn = pawn;
                mostValuablePawnValue = currentPawnValue;
            }
        }

        return mostValuablePawn;
    }

    
}