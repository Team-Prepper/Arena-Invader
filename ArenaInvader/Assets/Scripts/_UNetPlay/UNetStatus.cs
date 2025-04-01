using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using EHTool;

public class UNetStatus : NetworkBehaviour, IStatus
{

    [SerializeField]
    private NetworkVariable<int> _netMoney
        = new NetworkVariable<int>(0);
    [SerializeField]
    private NetworkVariable<int> _netHP
        = new NetworkVariable<int>(100);
    [SerializeField]
    private NetworkVariable<int> _netAtk
        = new NetworkVariable<int>(0);
    [SerializeField]
    private NetworkVariable<int> _netDfs
        = new NetworkVariable<int>(0);
    [SerializeField]
    private NetworkVariable<int> _netLevel
        = new NetworkVariable<int>(0);

    public int Money
    {
        get
        {
            return _netMoney.Value;
        }
        set
        {
            if (!IsOwner) return;
            _netMoney.Value = value;
        }
    }

    public int HP
    {
        get
        {
            return _netHP.Value;
        }
        set
        {
            if (!IsOwner) return;
            _netHP.Value = value;
        }
    }

    public int Atk
    {
        get
        {
            return _netAtk.Value + _levelStatus[_netLevel.Value].Atk;
        }
    }
    public int Dfs
    {
        get
        {
            return _netDfs.Value + _levelStatus[_netLevel.Value].Dfs;
        }
    }

    public bool IsAlive() => HP > 0;

    public string Name { get; set; } = "Tmp";

    private string _characterCode = "Player";

    [SerializeField] private StatusElement[] _levelStatus;

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

    private ISet<IObserver<IStatus>> _observers
        = new HashSet<IObserver<IStatus>>();

    ICharacterController _cc;

    public void SetCC(ICharacterController cc)
    {
        _cc = cc;
    }

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

    public override void OnNetworkSpawn()
    {
        _netMoney.OnValueChanged += (beforeValue, value) =>
        {
            Notify();
        };
        _netHP.OnValueChanged += (beforeValue, value) =>
        {
            Notify();
        };
        _netAtk.OnValueChanged += (beforeValue, value) =>
        {
            Notify();
        };
        _netDfs.OnValueChanged += (beforeValue, value) =>
        {
            Notify();
        };
    }

    public void LevelUp(int levelUpAmount)
    {
        if (!IsOwner) return;
        _netLevel.Value = levelUpAmount + _netLevel.Value;

    }

    public void AddAtk(int atk)
    {
        if (!IsOwner) return;
        _netAtk.Value = _netAtk.Value + atk;
    }

    public void AddDfs(int dfs)
    {
        if (!IsOwner) return;
        _netDfs.Value = _netDfs.Value + dfs;
    }

}