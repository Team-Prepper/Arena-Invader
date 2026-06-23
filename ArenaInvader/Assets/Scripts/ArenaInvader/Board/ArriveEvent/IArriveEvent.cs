using UnityEngine;
using System.Collections.Generic;
using System;

public class ArriveEventComparer : IComparer<ArriveEventBase> {
    public int Compare(ArriveEventBase x, ArriveEventBase y)
    {
        if (x == null && y == null) return 0;
        if (x == null) return 1;
        if (y == null) return -1;
        return x.GetPriority().CompareTo(y.GetPriority());
    }
}

public abstract class ArriveEventBase : MonoBehaviour {

    public abstract int GetPriority();

    public abstract void AddAbility(
        GamePawn target, Action callback);
    public abstract int GetValue(
        IStatus attacker, IStatus defender);
    
}
