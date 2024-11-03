using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlate : IPlate {

    [SerializeField] IPlate _nextPlate;

    public override void Arrive(Pawn target)
    {
        Debug.Log("¿¸≈ı UI");
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
