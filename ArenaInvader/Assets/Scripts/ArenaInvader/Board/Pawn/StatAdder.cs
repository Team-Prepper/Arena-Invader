using UnityEngine;

public class StatAdder : StatAdderBase
{

    public override void Reset()
    {
        
    }

    public override void LevelUp(IPlayableCharacter cc)
    {
        cc.Status.LevelUp(_levelCoefficient);

        Reset();
    }

    public override void AddAttack(
        IPlayableCharacter cc, int amount)
    {
        cc.Status.AddAtk(amount * _attackCoefficient);
    }

    public override void AddDefence(
        IPlayableCharacter cc, int amount)
    {
        cc.Status.AddDfs(amount * _defenseCoefficient);
    }

    public override void AddHealth(
        IPlayableCharacter cc, int amount)
    {
        cc.Status.AddHP(amount * _healthCoefficient);
    }
    public override void AddMoney(
        IPlayableCharacter cc, int amount)
    {
        cc.Status.AddMoney(amount * _moneyCoefficient);
    }
}