using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_PlusX : IItem
{
    [SerializeField] int _dicePoint = 3;
    
    public override void UseItem(IPlayableCharacter player)
    {
        player.GetExtraDicePoint(_dicePoint);
    }
}
