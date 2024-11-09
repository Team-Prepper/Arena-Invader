using EHTool.UIKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUITurnStart : GUIPopUp
{
    [SerializeField] Text _msg;
    [SerializeField] float _showTime = 1f;

    public void SetWaitForCallback(string msg, CallbackMethod callback)
    {
        SFXManager.Instance.PlaySFX("TurnStart");
        _msg.text = msg;
        StartCoroutine(WaitFor(_showTime, callback));
    }

    IEnumerator WaitFor(float maxTime, CallbackMethod callback) {
        yield return new WaitForSeconds(maxTime);

        callback?.Invoke();
    }

}
