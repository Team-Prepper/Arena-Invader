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

    [ServerRpc(RequireOwnership = false)]
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

    [ServerRpc(RequireOwnership = false)]
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
        LevelUpServerRpc(levelUpAmount);
    }

    public void AddMoney(int money)
    {
        AddMoneyServerRpc(money);
    }

    public void UseMoney(int money)
    {
        UseMoneyServerRpc(money);
    }

    public void AddHP(int hp)
    {
        AddHPServerRpc(hp);
    }

    public void TakeDamage(int damage)
    {
        TakeDamageServerRpc(damage);
    }

    public void AddAtk(int atk)
    {
        AddAtkServerRpc(atk);
    }

    public void AddDfs(int dfs)
    {
        AddDfsServerRpc(dfs);
    }

    [ServerRpc(RequireOwnership = false)]
    private void LevelUpServerRpc(int levelUpAmount)
    {
        _netLevel.Value = Mathf.Clamp(
            _netLevel.Value + levelUpAmount,
            0,
            GetMaxLevel());
    }

    [ServerRpc(RequireOwnership = false)]
    private void AddMoneyServerRpc(int money)
    {
        _netMoney.Value = Mathf.Max(0, _netMoney.Value + money);
    }

    [ServerRpc(RequireOwnership = false)]
    private void UseMoneyServerRpc(int money)
    {
        _netMoney.Value = Mathf.Max(0, _netMoney.Value - Mathf.Max(0, money));
    }

    [ServerRpc(RequireOwnership = false)]
    private void AddHPServerRpc(int hp)
    {
        _netHP.Value = Mathf.Max(0, _netHP.Value + hp);
    }

    [ServerRpc(RequireOwnership = false)]
    private void TakeDamageServerRpc(int damage)
    {
        _netHP.Value = Mathf.Max(0, _netHP.Value - Mathf.Max(0, damage));
    }

    [ServerRpc(RequireOwnership = false)]
    private void AddAtkServerRpc(int atk)
    {
        _netAtk.Value += atk;
    }

    [ServerRpc(RequireOwnership = false)]
    private void AddDfsServerRpc(int dfs)
    {
        _netDfs.Value += dfs;
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
