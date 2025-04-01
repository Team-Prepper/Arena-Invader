using System;
using System.Collections.Generic;
using UnityEngine;
using EHTool;

public class LocalStatus : MonoBehaviour, IStatus
{

    public string Name { get; set; } = "Tmp";

    private string _characterCode = "Player";

    private StatusElement[] _levelStatus;

    public string CharacterCode
    {
        get
        {
            return _characterCode;
        }
        set
        {
            _levelStatus = CharacterManager.Instance.GetStatuses(value);
            _characterCode = value;
        }
    }

    [SerializeField] private int _money = 0;
    [SerializeField] private int _hp = 100;
    [SerializeField] private int _atk = 0;
    [SerializeField] private int _dfs = 0;
    [SerializeField] private int _level = 0;

    public int Money
    {
        get
        {
            return _money;
        }
        set
        {
            _money = value;
            Notify();
        }
    }

    public int HP
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = value;
            Notify();
        }
    }

    public int Atk
    {
        get
        {
            return _atk + _levelStatus[_level].Atk;
        }
    }

    public int Dfs
    {
        get
        {
            return _dfs + _levelStatus[_level].Dfs;
        }
    }

    public bool IsAlive() => HP > 0;

    private ISet<IObserver<IStatus>> _observers
        = new HashSet<IObserver<IStatus>>();

    public IDisposable Subscribe(IObserver<IStatus> observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
            observer.OnNext(this);
        }
        return new Unsubscriber<IStatus>(_observers, observer);
    }

    public void Notify()
    {
        foreach (var o in _observers)
        {
            o.OnNext(this);
        }

    }

    public void LevelUp(int levelUpAmount)
    {
        _level += levelUpAmount;

    }
    
    public void AddAtk(int atk)
    {
        _atk += atk;

    }

    public void AddDfs(int dfs)
    {
        _dfs = dfs;
    }

}