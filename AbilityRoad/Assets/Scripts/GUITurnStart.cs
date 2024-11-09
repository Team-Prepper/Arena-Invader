using EHTool.UIKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUITurnStart : GUIPopUp
{
    [SerializeField] float _showTime = 1f;

    public void SetWaitForCallback(CallbackMethod callback) {
        StartCoroutine(WaitFor(_showTime, callback));
    }

    IEnumerator WaitFor(float maxTime, CallbackMethod callback) {
        yield return new WaitForSeconds(maxTime);

        callback?.Invoke();
    }

}
