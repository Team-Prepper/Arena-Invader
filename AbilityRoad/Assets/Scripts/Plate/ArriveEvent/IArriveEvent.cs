using UnityEngine;

public abstract class IArriveEvent : MonoBehaviour {
    abstract public void AddAbility(Pawn target, CallbackMethod callback);

    abstract public int GetValue();
}