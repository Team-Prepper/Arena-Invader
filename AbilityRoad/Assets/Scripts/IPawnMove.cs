using UnityEngine;

public abstract class IPawnMove : MonoBehaviour {

    public abstract void MoveTo(IPlate startPos, Pawn target, int amount, CallbackMethod arrive, CallbackMethod<IPlate> moveEnd);

    public abstract void DisposeTo(Vector3 pos);

}