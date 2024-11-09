using EHTool.UIKit;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GUIDice : GUIPopUp
{
    [SerializeField] Button _btn;
    [SerializeField] IDice _dice;

    CallbackMethod<int> _callback;

    bool _isRolling;

    public override void Open()
    {
        base.Open();
    }

    public void SetCallback(CallbackMethod<int> callback)
    {
        _callback = callback;
    }

    public void Roll() {
        if (_isRolling) return;

        _btn.enabled = false;
        _isRolling = true;

        _dice.Roll((amount) => {
            _callback?.Invoke(amount);
            Close();
        });
    }

}
