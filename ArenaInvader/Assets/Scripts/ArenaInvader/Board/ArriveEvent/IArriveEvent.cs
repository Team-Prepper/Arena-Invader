using UnityEngine;
using System.Collections.Generic;
using System;

public class IArriveEventComparer : IComparer<ArriveEventBase> {
    public int Compare(ArriveEventBase x, ArriveEventBase y)
    {
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