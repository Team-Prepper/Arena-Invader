using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherTypeStatAdder : StatAdderBase
{
    private int _attackValue = 0;
    private int _defenceValue = 0;
    private int _healthValue = 0;

    public override void Reset()
    {
        _healthValue = 0;
        _attackValue = 0;
        _defenceValue = 0;
        
    }

    public override void LevelUp(IPlayableCharacter cc)
    {
        cc.Status.LevelUp(_levelCoefficient);
        cc.Status.AddAtk(_attackValue);
        cc.Status.AddDfs(_defenceValue);
        cc.Status.AddHP(_healthValue);

        Reset();
    }

    public override void AddAttack(
        IPlayableCharacter cc, int amount)
    {
        _attackValue += amount * _attackCoefficient;
    }

    public override void AddDefence(
        IPlayableCharacter cc, int amount)
    {
        _defenceValue += amount * _defenseCoefficient;
    }

    public override void AddHealth(
        IPlayableCharacter cc, int amount)
    {
        _healthValue += amount * _healthCoefficient;
    }
    
    public override void AddMoney(
        IPlayableCharacter cc, int amount)
    {
        cc.Status.AddMoney(amount * _moneyCoefficient);
    }
}
