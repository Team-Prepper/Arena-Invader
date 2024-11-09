using UnityEngine;
using System.Collections.Generic;

public class IArriveEventComparer : IComparer<IArriveEvent> {
    public int Compare(IArriveEvent x, IArriveEvent y)
    {
        return x.GetPriority().CompareTo(y.GetPriority());
    }
}

public abstract class IArriveEvent : MonoBehaviour {

    public abstract int GetPriority();

    public abstract void AddAbility(Pawn target, CallbackMethod callback);
    public abstract int GetValue(Character attacker, Character defender);
}