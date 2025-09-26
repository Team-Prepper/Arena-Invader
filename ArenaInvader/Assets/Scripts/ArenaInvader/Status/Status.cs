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
        HP = CharacterManager.Instance.
            GetCharacterDefaultHP(characterCode);
        CharacterCode = characterCode;
    }

    public int Money { get; private set; } = 0;

    public int HP { get; private set; } = 100;

    public bool IsAlive() => HP > 0;

    public int Atk => _atk + _levelStatus[_level].Atk;

    public int Dfs => _dfs + _levelStatus[_level].Dfs;

    public void LevelUp(int levelUpAmount)
    {
        _level += levelUpAmount;
        Notify();

    }

    public void AddHP(int hp)
    {
        HP += hp;
        Notify();
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        Notify();
    }

    public void AddMoney(int money)
    {
        Money += money;
        Notify();
    }
    
    public void UseMoney(int money)
    {
        Money -= money;
        Notify();
    }

    public void AddAtk(int atk)
    {
        _atk += atk;
        Notify();

    }

    public void AddDfs(int dfs)
    {
        _dfs = dfs;
        Notify();
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
        if (!IsAlive())
        {
            OnDeathEvent?.Invoke();
        }
        
        foreach (var o in _observers)
        {
            o.OnNext(this);
        }

    }

}