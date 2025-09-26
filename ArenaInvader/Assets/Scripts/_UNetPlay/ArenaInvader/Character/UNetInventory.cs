using UnityEngine;
using System.Collections.Generic;

public class UNetInventory : MonoBehaviour, IInventory {

    private List<string> _items = new List<string>();
    public IList<string> Items => _items;

    private IPlayableCharacter _cc;
    
    public void SetCC(IPlayableCharacter cc)
    {
        _cc = cc;
    }

    public void UseItem(string item)
    {
        Debug.Log("USE ITEM!!");

        ItemManager.Instance.GetItemData(item).Item.UseItem(_cc);
        _items.Remove(item);
    }

    public void DiscardItem(string item)
    {
        _items.Remove(item);
    }

}