using UnityEngine;

public abstract class IPlate : MonoBehaviour {

    [SerializeField] ArriveEvent _event;
    
    IOverlapEvent _overlapEvent = new OneMoreChance();

    protected Pawn _nowPawn;

    abstract public void Leave(CallbackMethod<IPlate> callback);
    abstract public void NextPlate(IPlate from, CallbackMethod<IPlate> callback);

    public void Arrive(Pawn target, int amount = 1)
    {
        if (_event != null) {
            _event.AddAbility(target.GetOwner(), amount);
        }
        if (_nowPawn) {
            _overlapEvent?.Event(_nowPawn, target);
            return;
        }
        _nowPawn = target;
        target.GetOwner().EndTurn();
    }
}