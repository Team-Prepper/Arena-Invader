using EHTool.UIKit;
using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GUITurnStart : GUIWindow
{
    [SerializeField] private Text _msg;
    [SerializeField] private float _showTime = 1f;

    public void SetWaitForCallback(string msg, Action callback)
    {
        SFXManager.Instance.PlaySFX("TurnStart");
        _msg.text = msg;
        StartCoroutine(WaitFor(_showTime, callback));
    }

    public override void SetOff()
    {
        Close();
    }

    IEnumerator WaitFor(float maxTime, Action callback) {
        yield return new WaitForSeconds(maxTime);

        callback?.Invoke();
    }

}