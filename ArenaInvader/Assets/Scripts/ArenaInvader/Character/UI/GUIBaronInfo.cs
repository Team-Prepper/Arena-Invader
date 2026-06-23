using System;
using UnityEngine;
using UnityEngine.UI;

public class GUIBaronInfo : MonoBehaviour, IObserver<IStatus>
{
    [SerializeField] private Image _baronImage;
    [SerializeField] private Text _baronName;
    [SerializeField] private Text _baronHP;
    [SerializeField] private string _hpFormat = "{0}";

#nullable enable
    private IDisposable? _cancellation;
    private IStatus? _target;

    public void SetBaronInfo(IStatus character)
    {
        if (character == null)
        {
            Clear();
            return;
        }

        if (ReferenceEquals(_target, character))
        {
            UpdateView(character);
            return;
        }

        _cancellation?.Dispose();
        _target = character;
        _cancellation = character.Subscribe(this);
    }

    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(IStatus value)
    {
        UpdateView(value);
    }

    private void UpdateView(IStatus value)
    {
        if (_baronImage != null)
        {
            _baronImage.sprite =
                CharacterManager.Instance.GetCharacterSprites(value.CharacterCode).CharacterIcon;
        }

        if (_baronName != null)
        {
            _baronName.text = value.Name;
        }

        if (_baronHP != null)
        {
            _baronHP.text = string.Format(_hpFormat, value.HP);
        }
    }

    private void Clear()
    {
        _cancellation?.Dispose();
        _cancellation = null;
        _target = null;

        if (_baronName != null)
        {
            _baronName.text = string.Empty;
        }

        if (_baronHP != null)
        {
            _baronHP.text = string.Empty;
        }

        if (_baronImage != null)
        {
            _baronImage.sprite = null;
        }
    }

    private void OnDestroy()
    {
        _cancellation?.Dispose();
        _cancellation = null;
        _target = null;
    }
}
