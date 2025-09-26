using UnityEngine;
using EasyH.Gaming.PathBased;

public class GamePlate : Plate
{

    [SerializeField] MultipleArriveEvent _event;

    void Start()
    {
        _event = new MultipleArriveEvent(GetComponents<ArriveEventBase>());
        BoardManager.Instance.AddEvent(this, _event);
    }

    public int GetValue(IStatus attacker, IStatus defender)
    {
        return 0;
        //return _event.GetValue(GetPawn() as GamePawn, attacker, defender) + base.GetValue();
    }
    
}