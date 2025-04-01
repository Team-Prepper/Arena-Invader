using UnityEngine;
using System.Collections.Generic;
using System;

public class IArriveEventComparer : IComparer<IArriveEvent> {
    public int Compare(IArriveEvent x, IArriveEvent y)
    {
        return x.GetPriority().CompareTo(y.GetPriority());
    }
}

public abstract class IArriveEvent : MonoBehaviour {

    public abstract int GetPriority();
    public abstract void AddAbility(GamePawn target, Action callback);
    public abstract int GetValue(GamePawn attacker, GamePawn defender);
    
}