using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Robbery : IItem
{
    [SerializeField] int _robbingMoney = 10;
    
    public override void UseItem(BasePlayer player)
    {
        foreach (var user in GameManager.Instance.Playground.Players)
        {
            if(user == player) continue;
            player.Money += user.Money >= _robbingMoney ? _robbingMoney : user.Money;
            user.Money -= _robbingMoney;
        }
    }
}
