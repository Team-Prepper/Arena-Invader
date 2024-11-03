using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPlate : IPlate
{

    [SerializeField] IPlate _nextPlate;

    public override void Arrive(Pawn target)
    {
        Debug.Log("´É·Â È¹µæ");
        target.GetOwner().EndTurn();
    }

    public override void Leave(CallbackMethod<IPlate> callback)
    {
        callback?.Invoke(_nextPlate);
    }
    public override void NextPlate(IPlate from, CallbackMethod<IPlate> callback)
    {
        callback?.Invoke(_nextPlate);
    }
}
