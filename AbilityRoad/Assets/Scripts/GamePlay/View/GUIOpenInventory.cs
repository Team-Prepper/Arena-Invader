using System.Collections.Generic;
using UnityEngine;

public class GUIOpenInventory : GUINetworkPopUp<int> {

    [SerializeField] private List<GUIUnitItemSelect> _inventoryButtons =
        new List<GUIUnitItemSelect>();
    
    [Header("select Item Info")]
    [SerializeField] private GUIUnitItemDesc _itemDesc;

    private int _idx;

    private ICharacterController _cc;
    private IList<ItemData> _items;

    public void SetTarget(int value, int size, ICharacterController cc = null)
    {
        _cc = cc;
        _items = ItemManager.Instance.ItemListFromInt(value, size);
        InitInventory();
    }

    private void InitInventory()
    {
        _itemDesc.Disable();
        for (int i = 0; i < _inventoryButtons.Count; i++)
        {
            if (i < _items.Count)
            {
                _inventoryButtons[i].SetSlot(_items[i].Icon, i, SelectItemButton);
                continue;
            }
            _inventoryButtons[i].gameObject.SetActive(false);
        }

    }

    public void SelectItemButton(int idx) {

        if (!IsControlled) return;

        if (_idx >= 0) {
            _inventoryButtons[_idx].DisSelect();
        }

        SelectItem(idx);

    }

    public void Use() {
        if (!IsControlled) return;
        if (_idx < 0) return;

        ItemData currentItem = _items[_idx];
        
        _cc.Inventory.UseItem(currentItem);
        
        _inventoryButtons[_idx].DisableSlot();
        _itemDesc.Disable();

        _idx = -1;

    }

    public void Discard() {

        if (_idx < 0) return;

        _cc.Inventory.DiscardItem(_items[_idx]);

        _itemDesc.Disable();
        _inventoryButtons[_idx].DisableSlot();
        _idx = -1;

    }
    
    public void SelectItem(int idx)
    {
        _idx = idx;

        DisplaySelectItem(idx);
        NetworkModify(idx);

    }

    public override void NetworkModifiedEvent(int value)
    {
        if (IsControlled) return;
        DisplaySelectItem(value);
    }

    private void DisplaySelectItem(int idx) {

        if (idx < 0) return;

        ItemData currentItem = _items[idx];

        if (currentItem == null)
        {
            /*TODO Default IMG*/
            return;
        }

        _itemDesc.SetItemCode(currentItem);
    }

}
