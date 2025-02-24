using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using EHTool.LangKit;
using UnityEngine;
using UnityEngine.UI;

public class GUIShop : GUINetworkPopUp<int> {

    [SerializeField] private GameObject _inforGO;

    public int Size => _shopButtons.Length;

    [Header("Default Info")]
    [SerializeField] private Sprite _defaultIcon;
    [Header("Shop Info")]
    [SerializeField] private Image _selectItemIcon;
    [SerializeField] private Text _selectItemName;
    [SerializeField] private Text _selectItemPrice;
    [SerializeField] private Text _selectItemDescription;
    [SerializeField] private Button _buyButton;

    [Header("Shop Button")]
    [SerializeField] ShopUnit[] _shopButtons;

    private IList<ItemData> _currentSaleItems = new List<ItemData>();

    private Action _buyEvent;

    private ICharacterController _cc;

    public void SetBuyer(ICharacterController buyer)
    {
        _cc = buyer;
    }

    public void SetItems(int seed)
    {
        _inforGO.SetActive(false);
        _currentSaleItems = ItemManager.Instance.ItemListFromInt(seed, _shopButtons.Length);

        for (int i = 0; i < _shopButtons.Length; i++)
        {
            _shopButtons[i].SetSlot(_currentSaleItems[i]);
        }
    }

    public void SelectItemButton(int idx) {
        if (!IsControlled) return;

        SelectItem(idx);
    }

    public void SelectItem(int idx = -1)
    {
        _buyEvent = () =>
        {
            SFXManager.Instance.PlaySFX("ButtonSelect");
            PurchaseItem(idx);
            DisableSelectItem(idx);
            NetworkModify(-idx - 1);
        };

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
            _inforGO.SetActive(false);
            /*TODO Default IMG*/
            return;
        }

        _inforGO.SetActive(true);

        ItemData currentItem = _currentSaleItems[idx];

        _selectItemIcon.sprite = currentItem.Icon;
        _selectItemName.text = LangManager.Instance.GetStringByKey(currentItem.Name);
        _selectItemPrice.text = currentItem.Price.ToString();
        _selectItemDescription.text = currentItem.Desc;

    }

    private void DisableSelectItem(int idx)
    {
        if (idx != -1)
        {
            _shopButtons[idx].DisableSlot();
            return;
        }

        _selectItemIcon.sprite = _defaultIcon;
        _selectItemName.text = LangManager.Instance.GetStringByKey("Item_Empty"); ;
        _selectItemPrice.text = "0";
        _selectItemDescription.text = "None";

    }

    public void BuyButton()
    {
        if (!IsControlled) return;
        Buy();
    }

    public void Buy() {
        _buyEvent?.Invoke();
    }

    private void PurchaseItem(int idx)
    {
        ItemData item = _currentSaleItems[idx];
        if(item == null) return;
        if (_cc.Status.Money < item.Price) return;

        _cc.Status.Money -= item.Price;
        _cc.Status.Items.Add(item);
    }

    public IList<ItemData> GetSaleItems()
    {
        return _currentSaleItems;
    }

}
