using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using EasyH.Unity.UI;

public class GUITurnStart : GUIWindow
{
    [SerializeField] private Text _msg;
    [SerializeField] private float _showTime = 1f;

    public void SetMessage(string msg)
    { 
        _msg.text = msg;
        
    }

    public void SetWaitForCallback(Action callback)
    {
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