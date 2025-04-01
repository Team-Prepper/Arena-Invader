using System;
using UnityEngine;
using UnityEngine.UI;

public class GUIUnitPlayerButton : MonoBehaviour, IObserver<IStatus> {

    [System.Serializable]
    class FormattedTextUI {
        [SerializeField] private Text _textUI;
        [SerializeField] private string _format = "{0}";

        public void SetTextUI(object str) {
            if (_textUI == null) return;
            _textUI.text = string.Format(_format, str);
        }
    }

    [SerializeField] private GameObject _parent;
    [SerializeField] private Image _icon;

    [SerializeField] private FormattedTextUI _name;

    [SerializeField] private FormattedTextUI _health;

    [SerializeField] private FormattedTextUI _coin;

    [SerializeField] private FormattedTextUI _atk;

    [SerializeField] private FormattedTextUI _dfs;

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

        _health.SetTextUI(value.HP);
        _coin.SetTextUI(value.Money);
        _atk.SetTextUI(value.Atk);
        _dfs.SetTextUI(value.Dfs);
    }

    public void SetPlayer(IStatus target)
    {
        _cancellation = target.Subscribe(this);

        _icon.sprite = CharacterManager.Instance.GetCharacterSprites(target.CharacterCode).CharacterIcon;
        _target = target;

        _name.SetTextUI(target.Name);

    }

    public void OpenPlayerInfor() {
        if (_target == null) return;

        //GameObject.FindWithTag("PlayerInfor").GetComponent<GUIUnitPlayer>().SetPlayer(_target);
    }

}