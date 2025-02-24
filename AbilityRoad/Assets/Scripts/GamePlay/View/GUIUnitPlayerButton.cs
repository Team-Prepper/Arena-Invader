using System;
using UnityEngine;
using UnityEngine.UI;

public class GUIUnitPlayerButton : MonoBehaviour, IObserver<IStatus> {

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
    private IStatus? _target;

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
            _parent.SetActive(false);
            return;
        }
        _health.text = string.Format(_healthFormat, value.HP);
        _coin.text = string.Format(_coinFormat, value.Money);
    }

    public void SetPlayer(IStatus target)
    {
        _target = target;
        _name.text = string.Format(_nameFormat, target.Name);
        _icon.sprite = CharacterManager.Instance.GetCharacterSprites(target.CharacterCode).CharacterIcon;
        _cancellation = target.Subscribe(this);

    }

    public void OpenPlayerInfor() {
        if (_target == null) return;

        //GameObject.FindWithTag("PlayerInfor").GetComponent<GUIUnitPlayer>().SetPlayer(_target);
    }

}