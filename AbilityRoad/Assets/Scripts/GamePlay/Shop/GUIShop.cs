using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIShop : GUINetworkPopUp<int> {

    public int Size => _shopButtons.Length;

    [Header("Default Info")]
    [SerializeField] private Sprite _defaultIcon;
    [Header("Shop Info")]
    [SerializeField] private GUIUnitItemDesc _itemDesc;
    [SerializeField] private Text _selectItemPrice;
    [SerializeField] private Button _buyButton;

    [Header("Shop Button")]
    [SerializeField] private GUIUnitItemSelect[] _shopButtons;
    private int _idx;

    private IList<ItemData> _currentSaleItems = new List<ItemData>();

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
            _shopButtons[i].SetSlot(_currentSaleItems[i].Icon, i, SelectItemButton);
        }
    }

    public void SelectItemButton(int idx) {
        if (!IsControlled) return;

        if (_idx >= 0) {
            _shopButtons[_idx].DisSelect();
        }

        SelectItem(idx);
    }

    public void SelectItem(int idx = -1)
    {
        _idx = idx;

        DisplaySelectItem(idx);
        NetworkModify(idx);
    }

    public override void NetworkModifiedEvent(int value)
    {
        if (IsControlled) return;

        if (value >= 0)
        {
            DisplaySelectItem(value);
            return;
        }

        DisableSelectItem(-value + 1);
    }

    private void DisplaySelectItem(int idx)
    {
        if (idx == -1)
        {
            _itemDesc.Disable();
            return;
        }

        ItemData currentItem = _currentSaleItems[idx];

        _itemDesc.SetItemCode(currentItem);
        _selectItemPrice.text = currentItem.Price.ToString();

    }

    private void DisableSelectItem(int idx)
    {
        if (idx != -1)
        {
            _shopButtons[idx].DisableSlot();
            return;
        }

        _itemDesc.Disable();
        _selectItemPrice.text = "0";

    }

    public void BuyButton()
    {
        if (!IsControlled) return;
        Buy();
    }

    public void Buy() {
        SFXManager.Instance.PlaySFX("ButtonSelect");
        PurchaseItem(_idx);
        DisableSelectItem(_idx);
        NetworkModify(-_idx - 1);
    }

    private void PurchaseItem(int idx)
    {
        ItemData item = _currentSaleItems[idx];
        if(item == null) return;
        if (_cc.Status.Money < item.Price) return;

        _itemDesc.Disable();
        _cc.Status.Money -= item.Price;
        _cc.Inventory.Items.Add(item);
    }

    public IList<ItemData> GetSaleItems()
    {
        return _currentSaleItems;
    }

}
