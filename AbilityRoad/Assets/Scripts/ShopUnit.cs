using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUnit : MonoBehaviour
{
    [SerializeField] IItem _item;
    [SerializeField] int _price;
    
    [SerializeField] Text _nameText;
    [SerializeField] Text _priceText;
    [SerializeField] Button _buyButton;
    [SerializeField] Image _icon;
    
    public void SetItem(IItem item)
    {
        _item = item;
        _nameText.text = item.Name;
        _price = item.Price;
        _priceText.text = string.Format("{0}", _price);
        _icon.sprite = item.Icon;
    }
    
    public void SetBuyer(BasePlayer buyer)
    {
        _buyButton.onClick.RemoveAllListeners();
        _buyButton.onClick.AddListener(() =>
        {
            buyer.BuyItem(_item);
        });
    }
}
