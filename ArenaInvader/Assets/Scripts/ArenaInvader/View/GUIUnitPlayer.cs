using System;
using UnityEngine;
using UnityEngine.UI;

public class GUIUnitPlayer : MonoBehaviour, IObserver<IStatus> {

    [SerializeField] Image _icon;
    [SerializeField] Text _name;
    [SerializeField] string _nameFormat = "{0}";
    [SerializeField] Text _health;
    [SerializeField] string _healthFormat = "{0}";
    [SerializeField] Text _coin;
    [SerializeField] string _coinFormat = "{0}";
    [SerializeField] Text _attack;
    [SerializeField] string _attackFormat = "{0}";
    [SerializeField] Text _defense;
    [SerializeField] string _defenseFormat = "{0}";
    [SerializeField] CanvasGroup _canvasGroup;

    [SerializeField] GUIUnitShopUnit[] _shopButtons;

#nullable enable
    private IDisposable? _cancellation;
    private IStatus? _target;
    private IInventory? _inventory;
    private Action? _inventoryChangedHandler;

    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(IStatus value)
    {
        if (!value.IsAlive())
        {
            Close();
            return;
        }

        _name.text = string.Format(_nameFormat, value.Name);
        _health.text = string.Format(_healthFormat, value.HP);
        _coin.text = string.Format(_coinFormat, value.Money);
        _attack.text = string.Format(_attackFormat, value.Atk);
        _defense.text = string.Format(_defenseFormat, value.Dfs);
    }

    public void SetPlayer(IPlayableCharacter target)
    {
        if (target == null || target.Status == null)
        {
            Close();
            return;
        }

        UnsubscribeInventory();
        _inventory = target.Inventory;
        SubscribeInventory();
        SetPlayer(target.Status);
    }

    public void SetPlayer(IStatus target) {
        _cancellation?.Dispose();
        _target = target;
        _cancellation = target.Subscribe(this);

        _icon.sprite = CharacterManager.Instance.GetCharacterSprites(target.CharacterCode).CharacterIcon;
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
        RefreshInventory();

    }

    private void RefreshInventory()
    {
        for (int i = 0; i < _shopButtons.Length; i++)
        {
            if (_inventory == null || i >= _inventory.Items.Count)
            {
                _shopButtons[i].gameObject.SetActive(false);
                _shopButtons[i].DisableSlot();
                continue;
            }

            ItemData item = ItemManager.Instance.GetItemData(_inventory.Items[i]);
            _shopButtons[i].gameObject.SetActive(true);
            _shopButtons[i].SetSlot(item);
        }
    }

    public void Close()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
    }

    private void SubscribeInventory()
    {
        if (_inventory == null)
        {
            return;
        }

        _inventoryChangedHandler ??= RefreshInventory;
        _inventory.OnItemsChanged += _inventoryChangedHandler;
    }

    private void UnsubscribeInventory()
    {
        if (_inventory == null || _inventoryChangedHandler == null)
        {
            return;
        }

        _inventory.OnItemsChanged -= _inventoryChangedHandler;
    }

    private void OnDestroy()
    {
        _cancellation?.Dispose();
        UnsubscribeInventory();
        _cancellation = null;
        _target = null;
        _inventory = null;
        _inventoryChangedHandler = null;
    }

}
