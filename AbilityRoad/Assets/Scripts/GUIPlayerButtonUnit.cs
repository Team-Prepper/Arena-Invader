using EHTool.LangKit;
using EHTool.UIKit;
using System;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class GUIPlayerButtonUnit : MonoBehaviour, IObserver<Character> {

    [SerializeField] GameObject _parent;
    [SerializeField] Image _icon;
    [SerializeField] Text _name;
    [SerializeField] string _nameFormat = "{0}";
    [SerializeField] Text _health;
    [SerializeField] string _healthFormat = "{0}";
    [SerializeField] Text _coin;
    [SerializeField] string _coinFormat = "{0}";

#nullable enable
    private IDisposable? _cancellation;

#nullable enable
    BasePlayer _target;

    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(Character value)
    {
        if (!value.IsAlive())
        {
            _parent.SetActive(false);
            return;
        }
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
        GameObject.FindWithTag("PlayerInfor").GetComponent<GUIPlayerUnit>().SetPlayer(_target);
    }

}