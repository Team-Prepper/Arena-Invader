using System;
using UnityEngine;

public abstract class IDice : MonoBehaviour {

    public abstract void Initial();
    public abstract void Roll(Action<int> callback);

}
