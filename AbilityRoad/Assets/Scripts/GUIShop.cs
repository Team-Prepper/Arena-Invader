using System.Collections;
using System.Collections.Generic;
using EHTool.UIKit;
using UnityEngine;

public class GUIShop : GUIPopUp
{
    [SerializeField] GameObject _shopPanel;
    [SerializeField] private GameObject _shopUnitPrefab;
    [SerializeField] List<IItem> _saleItems;

    private CallbackMethod _callback;
    
    private int defaultDisplayCount = 5;

    private Transform _container;
    
    public void EnterShop(BasePlayer buyer, CallbackMethod callback)
    {
        _shopPanel.SetActive(true);
        ShowItems(buyer, _saleItems);
        _callback = callback;
    }
    
    private void ShowItems(BasePlayer buyer, List<IItem> items)
    {
        int displayCount = Mathf.Min(defaultDisplayCount, items.Count);
        for (int i = 0; i < displayCount; i++)
        {
            GameObject itemUnit = Instantiate(_shopUnitPrefab, _container);
            ShopUnit shopUnit = itemUnit.GetComponent<ShopUnit>();
            shopUnit.SetItem(items[i]);
            shopUnit.SetBuyer(buyer);
        }
    }
    
    public override void Close()
    {
        _callback?.Invoke();
        base.Close();
    }
}
