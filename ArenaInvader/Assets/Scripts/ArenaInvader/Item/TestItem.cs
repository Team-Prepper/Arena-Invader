using UnityEngine;

public class TestItem : IItem
{
    public override void UseItem(IPlayableCharacter player)
    {
        Debug.Log("USE ITEM");
    }
}