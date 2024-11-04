using UnityEngine;

public abstract class IArriveEvent : MonoBehaviour {
    abstract public void AddAbility(Pawn target, int amount, CallbackMethod callback);
}