using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIShop : GUINetworkPopUp<int> {

    public int Size => _shopButtons.Length;

    [Header("Shop Info")]
    [SerializeField] private GUIUnitItemDesc _itemDesc;
    [SerializeField] private Text _selectItemPrice;
    [SerializeField] private string _moneyErrorMessage = "돈이 부족합니다.";

    [Header("Shop Button")]
    [SerializeField] private GUIUnitItemSelect[] _shopButtons;
    private int _idx;

    private IList<string> _currentSaleItems = new List<string>();

    private Action _shopCloseEvent;
    private ICharacterController _cc;

    public void SetBuyer(ICharacterController buyer, Action shopCloseEvent)
    {
        _cc = buyer;
        _shopCloseEvent = shopCloseEvent;
    }

    public override void Close()
    {
        _shopCloseEvent?.Invoke();
        base.Close();
    }

    public void SetItems(int seed)
    {
        _itemDesc.Disable();
        _currentSaleItems = ItemManager.Instance.ItemListFromInt(seed, _shopButtons.Length);

        for (int i = 0; i < _shopButtons.Length; i++)
        {
            _shopButtons[i].SetSlot(_currentSaleItems, i, SelectItemButton);
        }
    }

    public void SelectItemButton(int idx) {
        if (!IsControlled) return;

        SelectItem(idx);
    }

    public void SelectItem(int idx = -1)
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

    private void DisplaySelectItem(int idx)
    {
        if (idx < 0)
        {
            _itemDesc.Disable();
            return;
        }

        if (_idx >= 0) {
            _shopButtons[_idx].SetLight(false);
        }

        _idx = idx;
        _shopButtons[idx].SetLight(true);

        ItemData currentItem = ItemManager.Instance.GetItemData(_currentSaleItems[idx]);

        _itemDesc.SetItemCode(currentItem);
        _selectItemPrice.text = currentItem.Price.ToString();

    }

    private void DisableSelectItem(int idx)
    {
        if (idx < 0)
        {
            return;
        }
        
        _shopButtons[idx].DisableSlot();
        _itemDesc.Disable();
        _selectItemPrice.text = "0";

        _idx = -1;

    }

    public void BuyButton()
    {
        if (!IsControlled) return;
        Buy();
    }

    public void Buy() {
        if (!PurchaseItem(_idx)) {
            EHTool.UIKit.UIManager.Instance.DisplayMessage(_moneyErrorMessage);
            return;
        }
        NetworkModify(-_idx - 1);
        DisableSelectItem(_idx);
    }

    private bool PurchaseItem(int idx)
    {
        ItemData item = ItemManager.Instance.GetItemData(_currentSaleItems[idx]);;
        if(item == null) return false;
        if (_cc.Status.Money < item.Price) return false;

        _itemDesc.Disable();
        _cc.Status.Money -= item.Price;
        _cc.Inventory.Items.Add(_currentSaleItems[idx]);

        return true;
    }

    public IList<string> GetSaleItems()
    {
        return _currentSaleItems;
    }

}
