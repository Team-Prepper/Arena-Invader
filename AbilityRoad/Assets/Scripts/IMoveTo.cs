using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IMoveTo : MonoBehaviour{
    public abstract void MoveTo(Vector3 pos, CallbackMethod callback);
}