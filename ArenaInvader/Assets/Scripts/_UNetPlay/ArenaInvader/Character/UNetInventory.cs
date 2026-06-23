using UnityEngine;
using System.Collections.Generic;
using System;

public class UNetInventory : MonoBehaviour, IInventory {
    public Action OnItemsChanged { get; set; }

    private List<string> _items = new List<string>();
    public IList<string> Items => _items;

    private IPlayableCharacter _cc;
    
    public void SetCC(IPlayableCharacter cc)
    {
        _cc = cc;
    }

    public void AddItem(string item)
    {
        _items.Add(item);
        OnItemsChanged?.Invoke();
    }

    public void UseItem(string item)
    {
        Debug.Log("USE ITEM!!");

        ItemManager.Instance.GetItemData(item).Item.UseItem(_cc);
        _items.Remove(item);
        OnItemsChanged?.Invoke();
    }

    public void DiscardItem(string item)
    {
        _items.Remove(item);
        OnItemsChanged?.Invoke();
    }

}
