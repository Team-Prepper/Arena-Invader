using UnityEngine;
using BoardGame;

public class GamePawn : Pawn {
    
    private int _attackValue = 0;
    private int _defenceValue = 0;
    private int _healthValue = 0;

    [SerializeField] int _attackCoefficient = 1;
    [SerializeField] int _defenseCoefficient = 1;
    [SerializeField] int _healthCoefficient = 1;
    [SerializeField] int _moneyCoefficient = 1;
    [SerializeField] int _moveCoefficient = 0;
    [SerializeField] int _levelCoefficient = 1;

    private ICharacterController _targetCC;

    public void SetTargetCC(ICharacterController target) {
        _targetCC = target;
    }

    public override Plate MovePredict(int amount) {
        return base.MovePredict(amount + _moveCoefficient);
    }

    public override void Move(int amount)
    {
        base.Move(amount + _moveCoefficient);
    }

    public override void Arrive()
    {
        base.Arrive();

        //_targetCC.Status.Atk += _attackValue;
        //_targetCC.Status.Dfs += _defenceValue;
        //_targetCC.Status.HP += _healthValue;
        
        _targetCC.Status.LevelUp(_levelCoefficient);

        ResetImproveState();
        
    }

    private void ResetImproveState()
    {
        _healthValue = 0;
        _attackValue = 0;
        _defenceValue = 0;
    }

    private GamePawn PawnToGamePawn(Pawn pawn) {
        return pawn as GamePawn;
    }
    
    public ICharacterController GetCharacterController() {
        return _targetCC;
    }

    public void AddMoney(int amount) {
        _targetCC.Status.Money += amount * _moneyCoefficient;
    }

    public void AddAttack(int attackAmount)
    {
        if (_piggyBacking != null)
            PawnToGamePawn(_piggyBacking).AddAttack(attackAmount);

        _targetCC.Status.AddAtk(attackAmount * _attackCoefficient);
        //_attackValue += attackAmount * _attackCoefficient;
    }

    public void AddDefence(int defenceAmount)
    {
        if (_piggyBacking != null)
            PawnToGamePawn(_piggyBacking).AddDefence(defenceAmount);
        
        _targetCC.Status.AddDfs(defenceAmount * _defenseCoefficient);
        //_defenceValue += defenceAmount * _defenseCoefficient;
    }

    public void AddHealth(int healAmount)
    {
        if (_piggyBacking != null)
            PawnToGamePawn(_piggyBacking).AddHealth(healAmount);

        _targetCC.Status.HP += healAmount * _healthCoefficient;
        //_healthValue += healAmount * _healthCoefficient;
    }
    
}