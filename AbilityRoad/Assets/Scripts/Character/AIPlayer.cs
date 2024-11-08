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
        int mostValuablePawnValue = -1;

        foreach (var pawn in _pawns)
        {
            if(pawn.IsPiggyBacked()) continue;
            IPlate plate = pawn.MovePredict(value);
            int currentPawnValue = plate == null ? 10 : plate.GetValue(this, SetTarget());
            if (currentPawnValue >= mostValuablePawnValue)
            {
                mostValuablePawn = pawn;
                mostValuablePawnValue = currentPawnValue;
            }
        }

        return mostValuablePawn;
    }

    private Character SetTarget()
    {
        Character target = null;
        int minHealth = 1000;
        foreach (var player in GameManager.Instance.Playground.Players)
        {
            if (player == this) continue;
            int currentHealth = player.GetHealth();
            if (minHealth < currentHealth)
            {
                minHealth = currentHealth;
                target = player;
            }
        }
        return target;
    }

    
}