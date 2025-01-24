using System;
using UnityEngine;

public abstract class IPlate : MonoBehaviour {

    [SerializeField] MultipleArriveEvent _event;
    
    IOverlapEvent _sameOwnerOverlapEvent = new PiggyBack();
    IOverlapEvent _otherOwnerOverlapEvent = new OneMoreAndBackHome();

    protected Pawn _nowPawn;

    [SerializeField] private GameObject _catchEffect;

    private void Start()
    {
        Initial();
    }

    protected virtual void Initial()
    {
        _event = new MultipleArriveEvent(GetComponents<IArriveEvent>());

    }

    public abstract void Leave(Pawn Target, Action<IPlate> callback);
    public abstract void NextPlate(IPlate from, Action<IPlate> callback);

    public void Arrive(Pawn pawn, Action callback = null)
    {
        if (!_nowPawn)
        {
            SetPawn(pawn);
            PlayEvent();
            return;
        }
        if (pawn.GetOwner() == _nowPawn.GetOwner())
        {
            _sameOwnerOverlapEvent?.Event(this, pawn);
            PlayEvent();
            return;
        }
        _otherOwnerOverlapEvent?.Event(this, pawn);
        SFXManager.Instance.PlaySFX("Kill");
        Instantiate(_catchEffect, transform.position + Vector3.up, Quaternion.identity);

        Invoke(nameof(PlayEvent), 1f);
    }

    void PlayEvent()
    {
        _event.Event(_nowPawn, () => {
            _nowPawn.GetOwner().EndTurn();

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