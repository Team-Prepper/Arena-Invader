using System.Collections;
using System.Collections.Generic;
using EHTool.LangKit;
using EHTool.UIKit;
using UnityEngine;
using UnityEngine.UI;

public class GUIShop : GUIPopUp
{
    [SerializeField] private GameObject _inforGO;

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
        _inforGO.SetActive(false);
        _currentSaleItems.Clear();
        for (int i = 0; i < _shopButtons.Length; i++)
        {
            IItem currentSlotItem = _saleItems[Random.Range(0, items.Count)];
            _currentSaleItems.Add(currentSlotItem);

            // 현재 인덱스 값을 로컬 변수에 저장
            int index = i;
    
            _shopButtons[i].SetSlot(currentSlotItem.Price, currentSlotItem.Icon, () => SelectItem(currentSlotItem, index));
        }
    }

    public void SelectItem(IItem currentItem, int buttonIndex = -1)
    {
        _inforGO.SetActive(true);
        if (currentItem == null)
        {
            /*TODO Default IMG*/
        }
        else
        {
            _selectItemIcon.sprite = currentItem.Icon;
            _selectItemName.text = LangManager.Instance.GetStringByKey(currentItem.Name);
            _selectItemPrice.text = currentItem.Price.ToString();
            _selectItemDescription.text = currentItem.Description;
        }
        
        _buyButton.onClick.RemoveAllListeners();
        _buyButton.onClick.AddListener(() =>
        {
            SFXManager.Instance.PlaySFX("ButtonSelect");
            PurchaseItem(currentItem, buttonIndex);
        });
    }
    
    private void PurchaseItem(IItem item, int buttonIndex)
    {
        if (_buyer.BuyItem(item))
        {
            Debug.Log("buttonIndex : " + buttonIndex);
            if(buttonIndex != -1)
                _shopButtons[buttonIndex].DisableSlot();
            
            _selectItemIcon.sprite = _defaultIcon;
            _selectItemName.text = LangManager.Instance.GetStringByKey("Item_Empty");;
            _selectItemPrice.text = "0";
            _selectItemDescription.text = "None";
            _buyButton.onClick.RemoveAllListeners();
        }
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
