using System;
using UnityEngine;
using EasyH.Unity.UI;

public class OpenShopUI : MonoBehaviour, IOpenShop
{
    private IPlayableCharacter _cc;
    
    public void Initial(IPlayableCharacter cc)
    {
        _cc = cc;
    }
    
    public GUIShop OpenShop(Action callback)
    {
        GUIShop shop = UIManager.Instance.OpenGUI<GUIShop>("Shop");

        shop.SetBuyer(_cc, callback);
        shop.SetItems(ItemManager.Instance.RandomItemListByInt(shop.Size));

        return shop;
    }

}