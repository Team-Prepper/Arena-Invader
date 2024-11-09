using EHTool.UIKit;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GUIPlayerUnit : GUIPopUp, IObserver<Character> {

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

    public void SetPlayer(Character target) {
        _cancellation?.Dispose();
        _cancellation = target.Subscribe(this);
    }

}