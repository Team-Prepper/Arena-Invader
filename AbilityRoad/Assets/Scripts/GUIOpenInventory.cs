using System.Collections;
using System.Collections.Generic;
using EHTool.LangKit;
using EHTool.UIKit;
using UnityEngine;
using UnityEngine.UI;

public class GUIOpenInventory : GUIPopUp
{
    [SerializeField] List<InventoryUnit> _inventoryButtons = new List<InventoryUnit>();
    
    [Header("select Item Info")]
    [SerializeField] private Image _selectItemIcon;
    [SerializeField] private Text _selectItemName;
    [SerializeField] private Text _selectItemDescription;
    [SerializeField] private Button _useButton;
    [SerializeField] private Button _discardButton;
    [SerializeField] private Button _rollDiceButton;

    private BasePlayer _owner;

    private void initInventory(List<IItem> inventoryItems)
    {
        for (int i = 0; i < _inventoryButtons.Count; i++)
        {
            if (i < inventoryItems.Count)
            {
                IItem item = inventoryItems[i];  // 로컬 변수로 캡처
                int index = i;
                _inventoryButtons[i].SetSlot(item.Icon, () => SelectItem(item, index));
            }
            else
            {
                _inventoryButtons[i].SetSlot(null, null);
            }
        }

        _rollDiceButton.onClick.RemoveAllListeners();
        _rollDiceButton.onClick.AddListener(() =>
        {
            SFXManager.Instance.PlaySFX("ButtonSelect");
            _owner.RollDice();
            Close();
        });
    }

    
    public void OpenInventory(BasePlayer user ,List<IItem> inventoryItems)
    {
        initInventory(inventoryItems);
        _owner = user;
    }
    
    private void SelectItem(IItem currentItem, int buttonIndex, CallbackMethod callback = null)
    {
        if (currentItem == null)
        {
            /*TODO Default IMG*/
        }
        else
        { 
            _selectItemIcon.sprite = currentItem.Icon;
            _selectItemName.text = LangManager.Instance.GetStringByKey(currentItem.Name);
            _selectItemDescription.text = currentItem.Description;
        }
        
        _useButton.onClick.RemoveAllListeners();
        _useButton.onClick.AddListener(() =>
        {
            SFXManager.Instance.PlaySFX("ButtonSelect");
            _owner.UseItem(currentItem, callback);
            _inventoryButtons[buttonIndex].DisableSlot();
            Close();
        });
        
        
        _discardButton.onClick.RemoveAllListeners();
        _discardButton.onClick.AddListener(() => {
            SFXManager.Instance.PlaySFX("ButtonSelect");
            _owner.DiscardItem(currentItem);
        });
    }
    
    
    
    
}
