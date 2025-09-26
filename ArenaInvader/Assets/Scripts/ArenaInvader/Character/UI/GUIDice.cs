using UnityEngine;
using UnityEngine.UI;
using System;
using EasyH.Unity.SoundKit;
//using BKTools;

public class GUIDice : GUINetworkPopUp<float>
{
    [SerializeField] private Button _btn;
    [SerializeField] private DiceBase _dice;

    [SerializeField] private Text _log;

    private Action<int> _callback;

    private bool _isRolling;

    public override void Open()
    {
        base.Open();
        _dice.SyncValue += NetworkModify;
    }

    public void SetSeed(int seed) {
        _dice.Initial(seed);

    }

    public void SetCallback(Action<int> callback)
    {
        _callback = callback;
    }

    public void RollButton() {
        if (!IsControlled) return;
        Roll();
    }

    public override void NetworkModifiedEvent(float value)
    {
        if (IsControlled) return;
        _dice.Shot(value);
    }

    public void Roll() {

        if (_isRolling) return;

        SoundManager.Instance.PlaySFX("Roll");
        _btn.enabled = false;
        _isRolling = true;

        _dice.Roll((amount) => {
            _callback?.Invoke(amount);
            Close();
        });
    }

}
