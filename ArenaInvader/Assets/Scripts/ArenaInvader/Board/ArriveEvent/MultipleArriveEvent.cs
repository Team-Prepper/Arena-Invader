using System;
using System.Collections.Generic;
using UnityEngine;
using EasyH.Gaming.PathBased;

public class MultipleArriveEvent {

    [SerializeField] List<ArriveEventBase> _event;

    Action _callback;
    GamePawn _pawn;
    int _idx;

    public MultipleArriveEvent(ArriveEventBase[] events) {
        _event = new List<ArriveEventBase>();

        for (int i = 0; i < events.Length; i++)
        {
            _event.Add(events[i]);
        }
        _event.Sort(new IArriveEventComparer());

    }

    public void Event(GamePawn pawn, Action callback) {
        _idx = 0;
        _pawn = pawn;
        _callback = callback;

        Callback();
    }

    void Callback()
    {
        if (_idx >= _event.Count) {
            _callback?.Invoke();
            _callback = null;
            return;
        }

        _event[_idx].AddAbility(_pawn, () => {
            _idx++;
            Callback();
        });
    }

    public int GetValue(PathEntity nowPawn, IStatus attacker, IStatus defender)
    {
        int plateValue = 0;

        foreach (var t in _event)
        {
            plateValue += t.GetValue(attacker, defender);
        }
        plateValue += nowPawn == null ? 0 : 10 /* _nowPawn.GetValue();*/;
        return plateValue;

    }

}