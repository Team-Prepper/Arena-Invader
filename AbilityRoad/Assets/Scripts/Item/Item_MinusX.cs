using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_MinusX : IItem
{
    [SerializeField] int _dicePoint = 3;
    
    public override void UseItem(BasePlayer player)
    {
        foreach (var user in GameManager.Instance.Playground.Players)
        {
            if(user == player) continue;
            user.GetExtraDicePoint(-_dicePoint);
        }
    }
}
