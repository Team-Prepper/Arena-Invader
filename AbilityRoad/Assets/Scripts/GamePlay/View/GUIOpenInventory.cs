using System;
using System.Collections.Generic;
using EHTool.LangKit;
using UnityEngine;
using UnityEngine.UI;

public class GUIOpenInventory : GUINetworkPopUp<int> {

    [SerializeField] List<InventoryUnit> _inventoryButtons = new List<InventoryUnit>();
    
    [Header("select Item Info")]
    [SerializeField] private Image _selectItemIcon;
    [SerializeField] private Text _selectItemName;
    [SerializeField] private Text _selectItemDescription;
    [SerializeField] private Button _useButton;
    [SerializeField] private Button _discardButton;
    [SerializeField] private Button _rollDiceButton;

    ICharacterController _cc;
    IList<ItemData> _items;

    public void SetTarget(int value, int size, ICharacterController cc = null)
    {
        _cc = cc;
        _items = ItemManager.Instance.ItemListFromInt(value, size);
        InitInventory();
    }

    private void InitInventory()
    {
        for (int i = 0; i < _inventoryButtons.Count; i++)
        {
            if (i < _items.Count)
            {
                ItemData item = _items[i];  // 로컬 변수로 캡처
                int index = i;
                _inventoryButtons[i].SetSlot(item.Icon, () => SelectItem(index));
                continue;
            }
            _inventoryButtons[i].SetSlot(null, null);
        }

        _rollDiceButton.onClick.RemoveAllListeners();
        _rollDiceButton.onClick.AddListener(() =>
        {
            SFXManager.Instance.PlaySFX("ButtonSelect");
            Close();
        });
    }
    
    public void SelectItem(int idx)
    {
        if (!IsControlled) return;

        ItemData currentItem = _items[idx];

        _useButton.onClick.RemoveAllListeners();
        _useButton.onClick.AddListener(() =>
        {
            UseItem(idx);
        });
        
        _discardButton.onClick.RemoveAllListeners();
        _discardButton.onClick.AddListener(() => {
            SFXManager.Instance.PlaySFX("ButtonSelect");
            _cc.Target.DiscardItem(currentItem);
        });

        DisplaySelectItem(idx);
        NetworkModify(idx);

    }

    public void UseItem(int idx)
    {
        if (!IsControlled) return;

        ItemData currentItem = _items[idx];
        _cc.Target.UseItem(currentItem);
        _inventoryButtons[idx].DisableSlot();

    }

    public override void NetworkModifiedEvent(int value)
    {
        if (IsControlled) return;
        DisplaySelectItem(value);
    }

    private void DisplaySelectItem(int idx) {

        ItemData currentItem = _items[idx];

        if (currentItem == null)
        {
            /*TODO Default IMG*/
            return;
        }

        _selectItemIcon.sprite = currentItem.Icon;
        _selectItemName.text = LangManager.Instance.GetStringByKey(currentItem.Name);
        _selectItemDescription.text = currentItem.Desc;
    }



}
