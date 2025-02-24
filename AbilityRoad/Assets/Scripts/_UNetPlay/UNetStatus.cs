using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using EHTool;

public class UNetStatus : NetworkBehaviour, IStatus {
    
    private NetworkVariable<int> _netMoney = new NetworkVariable<int>(0);
    private NetworkVariable<int> _netHP = new NetworkVariable<int>(100);
    private NetworkVariable<int> _netAtk = new NetworkVariable<int>(0);
    private NetworkVariable<int> _netDfs = new NetworkVariable<int>(0);

    public int Money {
        get {
            return _netMoney.Value;
        }
        set {
            if (!IsOwner) return;
            _netMoney.Value = value;
        }
    }

    public int HP {
        get {
            return _netHP.Value;
        }
        set {
            if (!IsOwner) return;
            _netHP.Value = value;
        }
    }

    public int Atk {
        get {
            return _netAtk.Value;
        }
        set {
            if (!IsOwner) return;
            _netAtk.Value = value;
        }
    }

    public int Dfs {
        get {
            return _netDfs.Value;
        }
        set {
            if (!IsOwner) return;
            _netDfs.Value = value;
        }
    }

    public bool IsAlive() => HP > 0;

    public string Name { get; set; } = "Tmp";
    public string CharacterCode { get; set; } = "Player";
    
    private List<ItemData> items = new List<ItemData>();
    public List<ItemData> Items { get; }

    private ISet<IObserver<IStatus>> _observers
        = new HashSet<IObserver<IStatus>>();

    ICharacterController _cc;

    public void SetCC(ICharacterController cc) {
        _cc = cc;
    }

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

    public override void OnNetworkSpawn() {
        _netMoney.OnValueChanged += (beforeValue, value) => {
            Notify();
        };
        _netHP.OnValueChanged += (beforeValue, value) => {
            Notify();
        };
        _netAtk.OnValueChanged += (beforeValue, value) => {
            Notify();
        };
        _netDfs.OnValueChanged += (beforeValue, value) => {
            Notify();
        };
    }

    public void LevelUp(int levelUpAmount) {


    }

    public void UseItem(ItemData item)
    {
        Debug.Log("USE ITEM!!");
        item.Item.UseItem(_cc);
        items.Remove(item);
    }

    public void DiscardItem(ItemData item)
    {
        items.Remove(item);
    }

}