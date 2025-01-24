using UnityEngine;

public class TestItem : IItem
{
    public override void UseItem(ICharacterController player)
    {
        Debug.Log("USE ITEM");
    }
}