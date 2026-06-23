using System;
using System.Collections.Generic;
using UnityEngine;
using EasyH;

public class Status : MonoBehaviour, IStatus
{
    public Action OnDeathEvent { get; set; }

    [SerializeField] private int _atk = 0;
    [SerializeField] private int _dfs = 0;
    [SerializeField] private int _level = 0;

    private StatusElement[] _levelStatus;
    private bool _deathNotified;

    public string Name { get; private set; } = "Tmp";
    public string CharacterCode { get; private set; }

    public void SetName(string name)
    {
        Name = name;
    }

    public void SetCharacter(string characterCode)
    {
        _levelStatus = CharacterManager.Instance.
            GetStatuses(characterCode);
        _level = Mathf.Clamp(_level, 0, GetMaxLevel());
        HP = CharacterManager.Instance.
            GetCharacterDefaultHP(characterCode);
        CharacterCode = characterCode;
        _deathNotified = false;
    }

    public int Money { get; private set; } = 0;

    public int HP { get; private set; } = 100;

    public bool IsAlive() => HP > 0;

    public int Atk => _atk + _levelStatus[GetSafeLevel()].Atk;

    public int Dfs => _dfs + _levelStatus[GetSafeLevel()].Dfs;

    public void LevelUp(int levelUpAmount)
    {
        _level = Mathf.Clamp(_level + levelUpAmount, 0, GetMaxLevel());
        Notify();

    }

    public void AddHP(int hp)
    {
        HP = Mathf.Max(0, HP + hp);
        Notify();
    }

    public void TakeDamage(int damage)
    {
        HP = Mathf.Max(0, HP - Mathf.Max(0, damage));
        Notify();
    }

    public void AddMoney(int money)
    {
        Money = Mathf.Max(0, Money + money);
        Notify();
    }
    
    public void UseMoney(int money)
    {
        Money = Mathf.Max(0, Money - Mathf.Max(0, money));
        Notify();
    }

    public void AddAtk(int atk)
    {
        _atk += atk;
        Notify();

    }

    public void AddDfs(int dfs)
    {
        _dfs += dfs;
        Notify();
    }

    private int GetSafeLevel()
    {
        if (_levelStatus == null || _levelStatus.Length == 0)
        {
            throw new InvalidOperationException(
                $"{nameof(Status)} on '{name}' does not have character status data.");
        }

        return Mathf.Clamp(_level, 0, _levelStatus.Length - 1);
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

}
