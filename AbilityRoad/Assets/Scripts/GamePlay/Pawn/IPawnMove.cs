using UnityEngine;
using System;


public abstract class IPawnMove : MonoBehaviour {

    public abstract void MoveTo(IPlate startPos, Pawn target, int amount, Action arrive, Action<IPlate> moveEnd);

    public abstract IPlate Predict(IPlate startPos, Pawn target, int amount);

    public abstract void DisposeTo(Vector3 pos);

}