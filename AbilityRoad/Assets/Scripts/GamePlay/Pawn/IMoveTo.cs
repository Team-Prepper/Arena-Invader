using System;
using UnityEngine;

public abstract class IMoveTo : MonoBehaviour{
    public abstract void MoveTo(Vector3 pos, Action callback);
}