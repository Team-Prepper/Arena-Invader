using System.Collections;
using System.Collections.Generic;
using EHTool.UIKit;
using UnityEngine;
using UnityEngine.UI;

public class GUIShop : GUIPopUp
{
    [Header("Shop Info")]
    Image _selectItemIcon;
    Text _selectItemName;
    Text _selectItemPrice;
    Text _selectItemDescription;
    Button _buyButton;
    
    [Header("Shop Button")]
    [SerializeField] ShopUnit[] _shopButtons;
    [SerializeField] List<IItem> _saleItems;

    private CallbackMethod _callback;
    
    private int defaultDisplayCount = 5;

    private Transform _container;
    private BasePlayer _buyer;
    
    public void EnterShop(BasePlayer buyer, CallbackMethod callback)
    {
        _buyer = buyer;
        SetItems(_saleItems);
        _callback = callback;
    }
    
    private void SetItems(List<IItem> items)
    {
        for (int i = 0; i < _shopButtons.Length; i++)
        {
            _shopButtons[i].SetSlot(items[i].Price, items[i].Icon, () => SelectItem(i));
        }
    }

    private void SelectItem(int itemIdx)
    {
        _selectItemIcon.sprite = _saleItems[itemIdx].Icon;
        _selectItemName.text = _saleItems[itemIdx].Name;
        _selectItemPrice.text = _saleItems[itemIdx].Price.ToString();
        _selectItemDescription.text = _saleItems[itemIdx].Description;
        
        _buyButton.onClick.RemoveAllListeners();
        _buyButton.onClick.AddListener(() => _buyer.BuyItem(_saleItems[itemIdx]));
    }

    public override void Close()
    {
        _callback?.Invoke();
        base.Close();
    }
}
