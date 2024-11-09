using EHTool.LangKit;
using EHTool.UIKit;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GUIPlayerButtonUnit : MonoBehaviour, IObserver<Character> {

    [SerializeField] Image _icon;
    [SerializeField] Text _name;
    [SerializeField] string _nameFormat = "{0}";
    [SerializeField] Text _health;
    [SerializeField] string _healthFormat = "{0}";
    [SerializeField] Text _coin;
    [SerializeField] string _coinFormat = "{0}";

#nullable enable
    private IDisposable? _cancellation;

    BasePlayer _target;

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
    }

    public void SetPlayer(BasePlayer target)
    {
        _target = target;
        _icon.sprite = CharacterManager.Instance.GetPlayerSpr(target.GetCharacterCode());
        _cancellation = target.Subscribe(this);

    }

    public void OpenPlayerInfor() {
        UIManager.Instance.OpenGUI<GUIPlayerUnit>("PlayerInfor").SetPlayer(_target);
    }

}