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

    public void ResetPawnAt(Plate nowPlate)
    { 
        if (nowPlate == null) return;

        if (!_pawnDict.ContainsKey(nowPlate))
        {
            _pawnDict.Add(nowPlate, null);
            return;
        }
        _pawnDict[nowPlate] = null;
    }

    public void OverlapProcess(Plate plate, GamePawn pawn)
    {
        if (plate == null || pawn == null) return;

        GamePawn defaultPawn = GetPawnAt(plate);

        if (defaultPawn == null)
        {
            SetPawnAt(plate, pawn);
            return;
        }

        if (defaultPawn.GetCC() == pawn.GetCC())
        {
            _sameTeamOverlapEvent.Event(plate, defaultPawn, pawn);
            return;
        }

        _otherTeamOverlapEvent.Event(plate, defaultPawn, pawn);

    }

    public void SetPawnAt(Plate plate, GamePawn pawn)
    { 
        if (plate == null) return;

        _pawnDict[plate] = pawn;
        
    }

    public void AbilityEvent(
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
}
