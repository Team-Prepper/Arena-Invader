using EHTool.UIKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPlayerUnit : MonoBehaviour, IObserver<Character> {
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

    [SerializeField] ShopUnit[] _shopButtons;

#nullable enable
    private IDisposable? _cancellation;

    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(Character value)
    {
        _name.text = string.Format(_nameFormat, value.GetName());
        _health.text = string.Format(_healthFormat, value.GetHealth());
        _coin.text = string.Format(_coinFormat, value.Money);
        _attack.text = string.Format(_attackFormat, value.GetAttackValue());
        _defense.text = string.Format(_defenseFormat, value.GetDefenseValue());
    }

    public void SetPlayer(BasePlayer target) {

        _icon.sprite = CharacterManager.Instance.GetPlayerSpr(target.GetCharacterCode());
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
        _cancellation?.Dispose();
        _cancellation = target.Subscribe(this);

        List<IItem> items = target.Items;

        for (int i = 0; i < _shopButtons.Length; i++)
        {
            if (i >= items.Count) {
                _shopButtons[i].gameObject.SetActive(false);
                continue;
            }
            _shopButtons[i].gameObject.SetActive(true);
            _shopButtons[i].SetSlot(0, items[i].Icon, () => { });
        }
    }

    public void Close()
    {
        _canvasGroup.alpha = 0;

    }

}