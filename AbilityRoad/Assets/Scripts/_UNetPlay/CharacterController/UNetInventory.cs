using UnityEngine;
using System.Collections.Generic;

public class UNetInventory : MonoBehaviour, IInventory {

    private List<ItemData> _items = new List<ItemData>();
    public IList<ItemData> Items => _items;

    private ICharacterController _cc;
    
    public void SetCC(ICharacterController cc)
    {
        _cc = cc;
    }

    public void UseItem(ItemData item)
    {
        Debug.Log("USE ITEM!!");

        item.Item.UseItem(_cc);
        _items.Remove(item);
    }

    public void DiscardItem(ItemData item)
    {
        _items.Remove(item);
    }

}