using EHTool;
using EHTool.LangKit;
using EHTool.UIKit;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IObservable<Character> {

    [SerializeField] protected string _name;
    [SerializeField] protected IGUIUnitHealth _healthUI;
    [SerializeField] protected Status _status;
    [SerializeField] protected int _health;
    [SerializeField] protected int _coin;

    private readonly ISet<IObserver<Character>> _observers = new HashSet<IObserver<Character>>();

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
        _healthUI.SetHealth(_health);

        Notify();
    }

    public void ReduceHealth(int amount)
    {
        _health -= amount;
        _healthUI.SetHealth(_health);
        Notify();

        if (_health >= 0) return;

        // ????? ?? ???? ???
    }

    protected virtual void DeathEvent() {}

    public bool IsAlive() {
        return _health > 0;
    }

    public void AddCoin(int amount) {
        _coin += amount;
        Notify();
    }

    public void ReduceCoin(int amount)
    {
        _coin = Mathf.Min(_coin - amount, 0);
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

    public int GetCoin() => _coin;

    public int GetAttackValue() => _status.GetAttackValue(_level);

    public int GetDefenseValue() => _status.GetDefenseValue(_level);

    public int GetHealth() => _health;
}