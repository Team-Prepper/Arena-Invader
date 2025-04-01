using System.Collections.Generic;
using UnityEngine;

public class GUIInventory : GUINetworkPopUp<int> {

    [SerializeField] private List<GUIUnitItemSelect> _inventoryButtons =
        new List<GUIUnitItemSelect>();
    
    [Header("select Item Info")]
    [SerializeField] private GUIUnitItemDesc _itemDesc;

    private int _idx;

    private ICharacterController _cc;
    private IList<string> _items;

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
            _inventoryButtons[i].SetSlot(_items, i, SelectItemButton);
        }

    }

    public void SelectItemButton(int idx) {

        if (!IsControlled) return;

        SelectItem(idx);

    }

    public void Use() {
        if (!IsControlled) return;
        if (_idx < 0) return;
        
        _cc.ShowUseItem(_items[_idx]);
        _cc.Inventory.UseItem(_items[_idx]);
        
        NetworkModify(-_idx - 1);
        DisableSelectItem(_idx);
    }

    public void Discard() {

        if (_idx < 0) return;

        _cc.Inventory.DiscardItem(_items[_idx]);

        NetworkModify(-_idx - 1);
        DisableSelectItem(_idx);

    }
    
    public void SelectItem(int idx)
    {
        NetworkModify(idx);
        DisplaySelectItem(idx);

    }

    public override void NetworkModifiedEvent(int value)
    {
        if (IsControlled) return;
        
        if (value >= 0)
        {
            DisplaySelectItem(value);
            return;
        }

        DisableSelectItem(-value - 1);
    }

    private void DisplaySelectItem(int idx) {

        if (idx < 0) {
            _itemDesc.Disable();
            return;
        }

        if (_idx >= 0) {
            _inventoryButtons[_idx].SetLight(false);
        }

        _idx = idx;
        _inventoryButtons[_idx].SetLight(true);

        _itemDesc.SetItemCode(_items[_idx]);
    }

    private void DisableSelectItem(int idx)
    {
        if (idx < 0)
        {
            return;
        }
        
        _inventoryButtons[idx].DisableSlot();
        _itemDesc.Disable();

        _idx = -1;

    }

}
