using UnityEngine;

public abstract class IPawnMove : MonoBehaviour {
    public abstract void MoveTo(Vector3 goal, float moveTime, float stopTime, CallbackMethod callback);
}