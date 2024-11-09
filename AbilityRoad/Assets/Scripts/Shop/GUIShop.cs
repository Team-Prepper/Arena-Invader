using System.Collections;
using System.Collections.Generic;
using EHTool.UIKit;
using UnityEngine;
using UnityEngine.UI;

public class GUIShop : GUIPopUp
{
    [Header("Shop Info")]
    [SerializeField] private Image _selectItemIcon;
    [SerializeField] private Text _selectItemName;
    [SerializeField] private Text _selectItemPrice;
    [SerializeField] private Text _selectItemDescription;
    [SerializeField] private Button _buyButton;
    
    [Header("Shop Button")]
    [SerializeField] ShopUnit[] _shopButtons;
    [SerializeField] List<IItem> _saleItems;
    
    private List<IItem> _currentSaleItems = new List<IItem>();

    private CallbackMethod _callback;

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
            IItem currentSlotItem = _saleItems[Random.Range(0,items.Count)];
            _currentSaleItems.Add(currentSlotItem);
            _shopButtons[i].SetSlot(currentSlotItem.Price, currentSlotItem.Icon, () => SelectItem(currentSlotItem));
        }
    }

    public void SelectItem(IItem currentItem)
    {
        if (currentItem == null)
        {
            /*TODO Default IMG*/
        }
        else
        { 
            _selectItemIcon.sprite = currentItem.Icon;
            _selectItemName.text = currentItem.Name;
            _selectItemPrice.text = currentItem.Price.ToString();
            _selectItemDescription.text = currentItem.Description;
        }
        
        
        _buyButton.onClick.RemoveAllListeners();
        _buyButton.onClick.AddListener(() => _buyer.BuyItem(currentItem));
    }
    
    public List<IItem> GetSaleItems()
    {
        return _currentSaleItems;
    }
    

    public override void Close()
    {
        _callback?.Invoke();
        base.Close();
    }
}
