using UnityEngine;
using UnityEngine.UI;
using System;

public class GUIDice : GUINetworkPopUp<int>
{
    [SerializeField] Button _btn;
    [SerializeField] IDice _dice;

    Action<int> _callback;

    bool _isRolling;

    public override void Open()
    {
        base.Open();
        _dice.Initial();
    }

    public void SetCallback(Action<int> callback)
    {
        _callback = callback;
    }

    public void Roll() {

        if (!IsControlled) return;
        if (_isRolling) return;

        SFXManager.Instance.PlaySFX("Roll");
        _btn.enabled = false;
        _isRolling = true;

        _dice.Roll((amount) => {
            _callback?.Invoke(amount);
            Close();
        });
    }

}
