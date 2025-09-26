using System;
using UnityEngine;
using EasyH.Unity.UI;

public class OpenInventoryUI : MonoBehaviour, IOpenInventory
{
    private IPlayableCharacter _cc;
    public void Initial(IPlayableCharacter cc)
    {
        _cc = cc;
    }

    public GUIInventory OpenInventory()
    {
        GUIInventory inventory = UIManager.Instance.OpenGUI<GUIInventory>("Inventory");

        //inventory.AddCloseMethod(_selector.RollDice);
        inventory.SetTarget(
            ItemManager.Instance.ItemListToInt(_cc.Inventory.Items),
            _cc.Inventory.Items.Count, _cc);

        return inventory;
    }

    public void ShowUseItem(string itemCode)
    { 
        UIManager.Instance.OpenGUI<GUIUseItemShow>(
            "UseItemShow").SetUseItem(itemCode);
        
    }
}