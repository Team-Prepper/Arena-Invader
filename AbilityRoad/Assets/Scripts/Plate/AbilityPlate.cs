using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPlate : IPlate
{
    [SerializeField] IPlate _nextPlate;

    public override void Leave(Pawn Target, CallbackMethod<IPlate> callback)
    {
        _nowPawn = null;
        callback?.Invoke(_nextPlate);
    }
    public override void NextPlate(IPlate from, CallbackMethod<IPlate> callback)
    {
        callback?.Invoke(_nextPlate);
    }
}
