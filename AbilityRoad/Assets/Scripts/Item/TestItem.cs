using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItem : IItem
{
    public override void UseItem(BasePlayer player)
    {
        Debug.Log("USE ITEM");
    }
}

