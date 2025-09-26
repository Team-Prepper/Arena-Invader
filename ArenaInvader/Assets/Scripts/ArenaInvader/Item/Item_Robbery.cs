using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Robbery : IItem
{
    [SerializeField] int _robbingMoney = 10;
    
    public override void UseItem(IPlayableCharacter player)
    {
        foreach (var user in GameManager.Instance.Playground.Players)
        {
            if(user == player) continue;
            
            player.Status.AddMoney(user.Status.Money >= _robbingMoney
                ? _robbingMoney : user.Status.Money);

            user.Status.UseMoney(_robbingMoney);
        }
    }
}
