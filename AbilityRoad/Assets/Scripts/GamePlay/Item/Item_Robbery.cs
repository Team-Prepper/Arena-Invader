using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Robbery : IItem
{
    [SerializeField] int _robbingMoney = 10;
    
    public override void UseItem(ICharacterController player)
    {
        foreach (var user in GameManager.Instance.Playground.Players)
        {
            if(user == player) continue;
            player.Target.Money += user.Target.Money >= _robbingMoney ? _robbingMoney : user.Target.Money;
            user.Target.Money -= _robbingMoney;
        }
    }
}
