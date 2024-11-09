using EHTool.LangKit;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GUIPlayerButtonUnit : MonoBehaviour, IObserver<Character> {

    [SerializeField] Text _name;
    [SerializeField] string _nameFormat = "{0}";
    [SerializeField] Text _health;
    [SerializeField] string _healthFormat = "{0}";
    [SerializeField] Text _coin;
    [SerializeField] string _coinFormat = "{0}";

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
        _name.text = string.Format(_nameFormat, value.GetHealth());
        _health.text = string.Format(_healthFormat, value.GetHealth());
        _coin.text = string.Format(_coinFormat, value.GetCoin());
    }

    public void SetPlayer(BasePlayer target)
    {
        _cancellation = target.Subscribe(this);

    }

}