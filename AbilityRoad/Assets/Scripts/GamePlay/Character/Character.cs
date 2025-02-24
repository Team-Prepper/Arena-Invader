using EHTool;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    
    [SerializeField] protected string _code = "Player";
    [SerializeField] protected int _health;

    private readonly ISet<IObserver<Character>> _observers = new HashSet<IObserver<Character>>();

    public string GetCharacterCode() => _code;

    protected virtual void DeathEvent() {}

    public bool IsAlive() {
        return _health > 0;
    }

}