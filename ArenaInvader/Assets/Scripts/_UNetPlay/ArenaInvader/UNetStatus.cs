using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using EHTool;

public class UNetStatus : NetworkBehaviour, IStatus
{
    public Action OnDeathEvent { get; set; }

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

    [SerializeField] private StatusElement[] _levelStatus;
    private bool _deathNotified;

    public int Money => _netMoney.Value;

    public int HP => _netHP.Value;

    public int Atk =>
        _netAtk.Value + _levelStatus[GetSafeLevel()].Atk;

    public int Dfs =>
         _netDfs.Value + _levelStatus[GetSafeLevel()].Dfs;

    public bool IsAlive() => HP > 0;

    public string Name { get; private set; } = "Tmp";
    public string CharacterCode { get; private set; }

    public void SetName(string name)
    {
        SetNameServerRpc(name);
    }

    [ServerRpc]
    private void SetNameServerRpc(string name)
    {
        SetNameClientRpc(name);
    }

    [ClientRpc]
    private void SetNameClientRpc(string name)
    {
        Name = name;
    }

    public void SetCharacter(string value)
    {
        SetCharacterServerRpc(value);
    }

    [ServerRpc]
    private void SetCharacterServerRpc(string value)
    {
        SetCharacterClientRpc(value);
        _netHP.Value = CharacterManager.Instance.
            GetCharacterDefaultHP(value);
    }

    [ClientRpc]
    public void SetCharacterClientRpc(string value)
    {
        CharacterCode = value;
        _levelStatus = CharacterManager.Instance.GetStatuses(value);
        _netLevel.Value = Mathf.Clamp(_netLevel.Value, 0, GetMaxLevel());
        _deathNotified = false;
    }

    public void LevelUp(int levelUpAmount)
    {
        if (!IsOwner) return;
        _netLevel.Value = Mathf.Clamp(_netLevel.Value + levelUpAmount, 0, GetMaxLevel());
        Notify();
    }

    public void AddMoney(int money)
    {
        if (!IsOwner) return;
        _netMoney.Value = Mathf.Max(0, _netMoney.Value + money);
        Notify();
    }

    public void UseMoney(int money)
    {
        if (!IsOwner) return;
        _netMoney.Value = Mathf.Max(0, _netMoney.Value - Mathf.Max(0, money));
        Notify();
    }

    public void AddHP(int hp)
    {
        if (!IsOwner) return;
        _netHP.Value = Mathf.Max(0, _netHP.Value + hp);
        Notify();
    }

    public void TakeDamage(int damage)
    {
        if (!IsOwner) return;
        _netHP.Value = Mathf.Max(0, _netHP.Value - Mathf.Max(0, damage));
        Notify();
    }

    public void AddAtk(int atk)
    {
        if (!IsOwner) return;
        _netAtk.Value += atk;
        Notify();
    }

    public void AddDfs(int dfs)
    {
        if (!IsOwner) return;
        _netDfs.Value += dfs;
        Notify();
    }

    private int GetSafeLevel()
    {
        if (_levelStatus == null || _levelStatus.Length == 0)
        {
            throw new InvalidOperationException(
                $"{nameof(UNetStatus)} on '{name}' does not have character status data.");
        }

        return Mathf.Clamp(_netLevel.Value, 0, _levelStatus.Length - 1);
    }

    private int GetMaxLevel()
    {
        return _levelStatus == null || _levelStatus.Length == 0
            ? 0
            : _levelStatus.Length - 1;
    }

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
        if (!IsAlive() && !_deathNotified)
        {
            _deathNotified = true;
            OnDeathEvent?.Invoke();
        }

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
        _netLevel.OnValueChanged += (beforeValue, value) =>
        {
            Notify();
        };
    }

}
