using System;
using UnityEngine;
using UnityEngine.UI;

public class GUIUnitPlayerButton : MonoBehaviour, IObserver<IStatus> {

    [Serializable]
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
    private IPlayableCharacter? _character;
    private GUIUnitPlayer? _playerInfoPanel;

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

    public void SetPlayer(IPlayableCharacter target)
    {
        _character = target;
        SetPlayer(target.Status);
    }

    public void SetPlayer(IStatus target)
    {
        _cancellation?.Dispose();
        _cancellation = target.Subscribe(this);

        _icon.sprite = CharacterManager.Instance.GetCharacterSprites(target.CharacterCode).CharacterIcon;
        _target = target;

        _name.SetTextUI(target.Name);

    }

    public void SetPlayerInfoPanel(GUIUnitPlayer playerInfoPanel)
    {
        _playerInfoPanel = playerInfoPanel;
    }

    public void OpenPlayerInfor() {
        if (_character == null && _target == null) return;
        if (_playerInfoPanel == null) return;

        if (_character != null)
        {
            _playerInfoPanel.SetPlayer(_character);
            return;
        }

        _playerInfoPanel.SetPlayer(_target);
    }

    private void OnDestroy()
    {
        _cancellation?.Dispose();
        _cancellation = null;
        _target = null;
        _character = null;
        _playerInfoPanel = null;
    }

}
