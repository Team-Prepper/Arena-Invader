using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IDice : MonoBehaviour {

    public abstract void Initial();
    public abstract void Roll(CallbackMethod<int> callback);

}
