using EHTool.UIKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIRaid : GUIFullScreen {

    CallbackMethod _callback;

    public void StartRaid(IPawnMove attacker, IPlate plate, CallbackMethod callback) {
        _callback = callback;
        _callback?.Invoke();
    }
}
