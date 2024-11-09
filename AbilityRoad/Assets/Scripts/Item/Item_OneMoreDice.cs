using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_OneMoreDice : IItem
{
    public override void UseItem(BasePlayer player)
    {
        player.AddChance();
    }
}
