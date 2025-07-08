using UnityEngine;
using BoardGame;

public class GamePlate : Plate
{

    [SerializeField] MultipleArriveEvent _event;

    void Start()
    {
        _event = new MultipleArriveEvent(GetComponents<IArriveEvent>());
    }

    protected override void PlayEvent()
    {
        GamePawn gamePawn = GetPawn() as GamePawn;

        if (gamePawn == null)
        {
            base.PlayEvent();
            return;
        }

        _event.Event(gamePawn, () =>
        {
            base.PlayEvent();
        });

    }

    public int GetValue(IStatus attacker, IStatus defender)
    {
        return _event.GetValue(GetPawn() as GamePawn, attacker, defender)
            + base.GetValue();
    }
    
}