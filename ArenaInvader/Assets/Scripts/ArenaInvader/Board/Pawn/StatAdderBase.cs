using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatAdderBase : MonoBehaviour
{

    [SerializeField] protected int _attackCoefficient = 1;
    [SerializeField] protected int _defenseCoefficient = 1;
    [SerializeField] protected int _healthCoefficient = 1;
    [SerializeField] protected int _moneyCoefficient = 1;
    [SerializeField] protected int _levelCoefficient = 1;
    
    public abstract void Reset();

    public abstract void LevelUp(IPlayableCharacter cc);

    public abstract void AddAttack(
        IPlayableCharacter cc, int amount);

    public abstract void AddDefence(
        IPlayableCharacter cc, int amount);

    public abstract void AddHealth(
        IPlayableCharacter cc, int amount);

    public abstract void AddMoney(
        IPlayableCharacter cc, int amount);
}
