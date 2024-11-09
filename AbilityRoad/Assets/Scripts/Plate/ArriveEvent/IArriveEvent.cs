using UnityEngine;

public abstract class IArriveEvent : MonoBehaviour {

    public abstract int GetPriority();

    public abstract void AddAbility(Pawn target, CallbackMethod callback);
    public abstract int GetValue(Character attacker, Character defender);
}