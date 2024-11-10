using EHTool;
using EHTool.LangKit;
using EHTool.UIKit;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Character : MonoBehaviour, IObservable<Character> {

    [SerializeField] protected string _name;
    [SerializeField] protected string _code = "Player";
    [SerializeField] protected Status _status;
    [SerializeField] protected int _health;
    [SerializeField] protected int _money;

    public int Money{
        get => _money;
        set {
            _money = value;
            if(_money < 0) _money = 0;
            Notify();
        }
    }

    private readonly ISet<IObserver<Character>> _observers = new HashSet<IObserver<Character>>();

    public string GetCharacterCode() => _code;

    public IDisposable Subscribe(IObserver<Character> observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);

            observer.OnNext(this);
        }

        return new Unsubscriber<Character>(_observers, observer);
    }

    void Notify() {

        foreach (IObserver<Character> target in _observers)
        {
            target.OnNext(this);
        }

    }

    protected int _level;

    internal void AddHeal(int healAmount)
    {
        _health += healAmount;

        Notify();
    }

    public void ReduceHealth(int amount)
    {
        _health -= amount;
        Notify();

        if (_health >= 0) return;

        DeathEvent();
        gameObject.SetActive(false);
    }

    protected virtual void DeathEvent() {}

    public bool IsAlive() {
        return _health > 0;
    }

    public void AddMoney(int amount) {
        _money += amount;
        Notify();
    }

    public void ReduceMoney(int amount)
    {
        _money = Mathf.Min(_money - amount, 0);
        Notify();
    }
    internal void AddAttack(int attackAmount)
    {
        _status.AddAttackValue(attackAmount);
        Notify();
    }

    internal void AddDefence(int defenceAmount)
    {
        _status.AddDefenceValue(defenceAmount);
        Notify();
    }

    public string GetName() => _name;

    public int GetAttackValue() => _status.GetAttackValue(_level);

    public int GetDefenseValue() => _status.GetDefenseValue(_level);

    public int GetHealth() => _health;
}