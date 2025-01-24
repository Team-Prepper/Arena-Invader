using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPlate : IPlate
{
    [SerializeField] IPlate _nextPlate;

    public override void Leave(Pawn Target, Action<IPlate> callback)
    {
        _nowPawn = null;
        callback?.Invoke(_nextPlate);
    }
    public override void NextPlate(IPlate from, Action<IPlate> callback)
    {
        callback?.Invoke(_nextPlate);
    }
}
