using UnityEngine;

public abstract class IPlate : MonoBehaviour {
    abstract public void Leave(CallbackMethod<IPlate> callback);
    abstract public void NextPlate(IPlate from, CallbackMethod<IPlate> callback);
    abstract public void Arrive(Pawn target);
}