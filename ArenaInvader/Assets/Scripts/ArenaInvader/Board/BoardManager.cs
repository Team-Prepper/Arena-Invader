using System;
using System.Collections.Generic;
using EasyH;
using EasyH.Gaming.PathBased;

public class BoardManager : Singleton<BoardManager>
{
    private IOverlapEvent _sameTeamOverlapEvent
        = new PiggyBack();
    private IOverlapEvent _otherTeamOverlapEvent
        = new OneMoreAndBackHome();

    private IDictionary<Plate, MultipleArriveEvent> _eventDict;
    private IDictionary<Plate, GamePawn> _pawnDict;

    public GameMap Map { get; set; }

    protected override void OnCreate()
    {
        _eventDict = new Dictionary
            <Plate, MultipleArriveEvent>();

        _pawnDict = new Dictionary<Plate, GamePawn>();
    }

    public void AddEvent(Plate gamePlate,
        MultipleArriveEvent arriveEvent)
    {
        if (gamePlate == null || arriveEvent == null) return;

        _eventDict[gamePlate] = arriveEvent;
    }

    public void ClearPawnAt(Plate plate)
    {
        if (plate == null) return;
        _pawnDict[plate] = null;
    }

    // Applies board occupancy rules when a pawn finishes moving onto a plate.
    public void ResolvePawnArrival(Plate plate, GamePawn pawn)
    {
        if (plate == null || pawn == null) return;

        GamePawn currentPawn = GetPawnAt(plate);

        if (currentPawn == null)
        {
            OccupyPawnAt(plate, pawn);
            return;
        }

        if (currentPawn.GetCC() == pawn.GetCC())
        {
            _sameTeamOverlapEvent.Event(plate, currentPawn, pawn);
            return;
        }

        _otherTeamOverlapEvent.Event(plate, currentPawn, pawn);
    }

    public void OccupyPawnAt(Plate plate, GamePawn pawn)
    {
        if (plate == null) return;
        _pawnDict[plate] = pawn;
    }

    // Runs arrive-event abilities in order, then hands control back to the caller.
    public void RunArriveEvents(
        Plate plate, GamePawn pawn, Action callback)
    {
        if (plate != null && _eventDict.TryGetValue(plate, out MultipleArriveEvent arriveEvent))
        {
            arriveEvent.Event(pawn, callback);
            return;
        }

        callback?.Invoke();
    }

    public GamePawn GetPawnAt(Plate plate)
    {
        if (plate == null) return null;
        _pawnDict.TryGetValue(plate, out GamePawn pawn);
        return pawn;
    }

    public void ResetPawnAt(Plate nowPlate)
    {
        ClearPawnAt(nowPlate);
    }

    public void OverlapProcess(Plate plate, GamePawn pawn)
    {
        ResolvePawnArrival(plate, pawn);
    }

    public void SetPawnAt(Plate plate, GamePawn pawn)
    {
        OccupyPawnAt(plate, pawn);
    }

    public void AbilityEvent(
        Plate plate, GamePawn pawn, Action callback)
    {
        RunArriveEvents(plate, pawn, callback);
    }
}
