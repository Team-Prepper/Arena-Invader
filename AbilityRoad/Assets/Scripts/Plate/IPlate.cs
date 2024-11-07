using Unity.VisualScripting;
using UnityEngine;

public abstract class IPlate : MonoBehaviour {

    [SerializeField] IArriveEvent _event;
    
    IOverlapEvent _sameOwnerOverlapEvent = new PiggyBack();
    IOverlapEvent _otherOwnerOverlapEvent = new OneMoreAndBackHome();
    //IOverlapEvent _otherOwnerOverlapEvent = new BackHome();
    //IOverlapEvent _otherOwnerOverlapEvent = new OneMoreChance();

    protected Pawn _nowPawn;

    private void Start()
    {
        _event = GetComponent<IArriveEvent>();
    }

    abstract public void Leave(Pawn Target, CallbackMethod<IPlate> callback);
    abstract public void NextPlate(IPlate from, CallbackMethod<IPlate> callback);

    public void Arrive(Pawn pawn, CallbackMethod callback = null)
    {
        void EventCallback()
        {
            if (!_nowPawn)
            {
                SetPawn(pawn);
                callback?.Invoke();
                return;
            }
            if (pawn.GetOwner() == _nowPawn.GetOwner())
            {
                _sameOwnerOverlapEvent?.Event(this, pawn);
                return;
            }
            _otherOwnerOverlapEvent?.Event(this, pawn);
        }

        if (_event == null) {
            EventCallback();
            return;
        }

        _event.AddAbility(pawn, EventCallback);
    }

    public void SetPawn(Pawn pawn) {
        _nowPawn = pawn;
    }

    public Pawn GetPawn() {
        return _nowPawn;
    }
}