using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class IPlate : MonoBehaviour {

    [SerializeField] MultipleArriveEvent _event;
    
    IOverlapEvent _sameOwnerOverlapEvent = new PiggyBack();
    IOverlapEvent _otherOwnerOverlapEvent = new OneMoreAndBackHome();
    //IOverlapEvent _otherOwnerOverlapEvent = new BackHome();
    //IOverlapEvent _otherOwnerOverlapEvent = new OneMoreChance();

    protected Pawn _nowPawn;

    [SerializeField] private GameObject _catchEffect;

    protected virtual void Initial()
    {
        _event = new MultipleArriveEvent(GetComponents<IArriveEvent>());

    }

    private void Start()
    {
        Initial();
    }

    public abstract void Leave(Pawn Target, CallbackMethod<IPlate> callback);
    public abstract void NextPlate(IPlate from, CallbackMethod<IPlate> callback);

    public void Arrive(Pawn pawn, CallbackMethod callback = null)
    {
        _event.Event(pawn, () =>
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
            SFXManager.Instance.PlaySFX("Kill");
            Instantiate(_catchEffect, transform.position + Vector3.up, Quaternion.identity);
        });
    }

    public void SetPawn(Pawn pawn) {
        _nowPawn = pawn;
    }

    public Pawn GetPawn() {
        return _nowPawn;
    }

    public virtual int GetValue(Character attacker, Character defender)
    {
        return _event.GetValue(_nowPawn, attacker, defender);
    }
}