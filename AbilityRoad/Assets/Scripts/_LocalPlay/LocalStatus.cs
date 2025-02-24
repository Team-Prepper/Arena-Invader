using System;
using System.Collections.Generic;
using UnityEngine;
using EHTool;

public class LocalStatus : MonoBehaviour, IStatus {

    public string Name { get; set; } = "Tmp";
    public string CharacterCode { get; set; } = "Player";

    [SerializeField] private int _money = 0;
    [SerializeField] private int _hp = 100;
    [SerializeField] private int _atk = 0;
    [SerializeField] private int _dfs = 0;

    ICharacterController _cc;

    public void SetCC(ICharacterController cc) {
        _cc = cc;
    }

    public int Money {
        get {
            return _money;
        }
        set {
            _money = value;
            Notify();
        }
    }

    public int HP {
        get {
            return _hp;
        }
        set {
            _hp = value;
            Notify();
        }
    }

    public int Atk {
        get {
            return _atk;
        }
        set {
            _atk = value;
            Notify();
        }
    }

    public int Dfs {
        get {
            return _dfs;
        }
        set {
            _dfs = value;
            Notify();
        }
    }

    public bool IsAlive() => HP > 0;

    private List<ItemData> _items = new List<ItemData>();
    public List<ItemData> Items => _items;

    private ISet<IObserver<IStatus>> _observers
        = new HashSet<IObserver<IStatus>>();

    public IDisposable Subscribe(IObserver<IStatus> observer) {
        if (!_observers.Contains(observer)) {
            _observers.Add(observer);
            observer.OnNext(this);
        }
        return new Unsubscriber<IStatus>(_observers, observer);
    }

    public void Notify() {
        foreach(var o in _observers) {
            o.OnNext(this);
        }

    }

    public void LevelUp(int levelUpAmount) {
        
        
    }

    public void UseItem(ItemData item)
    {
        Debug.Log("USE ITEM!!");

        item.Item.UseItem(_cc);
        _items.Remove(item);
        //item.Item.UseItem(_cc);
    }

    public void DiscardItem(ItemData item)
    {
        _items.Remove(item);
    }
}