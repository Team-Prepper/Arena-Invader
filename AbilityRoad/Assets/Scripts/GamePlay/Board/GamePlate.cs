using UnityEngine;
using BoardGame;

public class GamePlate : Plate {

    [SerializeField] MultipleArriveEvent _event;

    void Start()
    {
        _event = new MultipleArriveEvent(GetComponents<IArriveEvent>());
    }

    protected override void PlayEvent()
    {
        GamePawn gamePawn = GetPawn() as GamePawn;

        if (gamePawn == null) {
            base.PlayEvent();
            return;
        }

        _event.Event(gamePawn, () => {
            base.PlayEvent();
        });

    }

    public virtual int GetValue(IStatus attacker, IStatus defender)
    {
        return 0;
        //return _event.GetValue(_nowPawn, attacker, defender);
    }
}