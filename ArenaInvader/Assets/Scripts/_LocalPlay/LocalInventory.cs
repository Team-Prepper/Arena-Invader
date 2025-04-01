using UnityEngine;
using System.Collections.Generic;

public class LocalInventory : MonoBehaviour, IInventory {

    private List<string> _items = new List<string>();
    public IList<string> Items => _items;

    private ICharacterController _cc;
    
    public void SetCC(ICharacterController cc)
    {
        _cc = cc;
    }

    public void UseItem(string item)
    {
        ItemManager.Instance.GetItemData(item).Item.UseItem(_cc);
        _items.Remove(item);
    }

    public void DiscardItem(string item)
    {
        _items.Remove(item);
    }

}