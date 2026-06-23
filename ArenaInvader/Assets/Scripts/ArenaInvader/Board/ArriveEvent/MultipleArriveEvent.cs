using System;
using System.Collections.Generic;
using UnityEngine;
using EasyH.Gaming.PathBased;

public class MultipleArriveEvent {

    [SerializeField] List<ArriveEventBase> _event;

    private Action _callback;
    private GamePawn _pawn;
    private int _idx;

    public MultipleArriveEvent(ArriveEventBase[] events) {
        _event = new List<ArriveEventBase>();

        if (events == null)
        {
            return;
        }

        for (int i = 0; i < events.Length; i++)
        {
            if (events[i] == null) continue;
            _event.Add(events[i]);
        }

        _event.Sort(new ArriveEventComparer());

    }

    public void Event(GamePawn pawn, Action callback) {
        if (pawn == null)
        {
            callback?.Invoke();
            return;
        }

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
            _pawn = null;
            return;
        }

        ArriveEventBase current = _event[_idx];
        if (current == null)
        {
            _idx++;
            Callback();
            return;
        }

        current.AddAbility(_pawn, () =>
        {
            _idx++;
            Callback();
        });
    }

    public int GetValue(PathEntity nowPawn, IStatus attacker, IStatus defender)
    {
        int plateValue = 0;

        foreach (var t in _event)
        {
            if (t == null) continue;
            plateValue += t.GetValue(attacker, defender);
        }
        plateValue += nowPawn == null ? 0 : 10 /* _nowPawn.GetValue();*/;
        return plateValue;

    }

}
