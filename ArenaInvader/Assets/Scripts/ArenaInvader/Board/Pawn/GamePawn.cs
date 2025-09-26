using UnityEngine;
using EasyH.Gaming.PathBased;
using EasyH.Unity.SoundKit;
using System;

[RequireComponent(typeof(StatAdderBase))]
public class GamePawn : PathEntity
{
    public int Id { get; set; }

    [SerializeField] private PawnSelectBase _select;
    [SerializeField] private PawnTeamIdxSetterBase _idxSetter;
    [SerializeField] private StatAdderBase _statAdder;
    [SerializeField] private int _moveCoefficient = 0;

    private IPawnOwner _owner;
    private GamePawn _piggyBacking;
    private bool _isPiggyBacked = false;

    private IPlayableCharacter _targetCC;

    public IPlayableCharacter GetCC() => _targetCC;

    private void Start()
    {
        _statAdder = GetComponent<StatAdderBase>();
    }

    public void SetTargetCC(IPlayableCharacter target)
    {
        _targetCC = target;
        _idxSetter.SetTeamIdx(target.TurnState.TeamIdx);
    }

    public void SetOwner(IPawnOwner pawnOwner)
    {
        _owner = pawnOwner;
    }

    public Plate MovePredict(int amount)
    {
        Plate retval = _nowPlate;

        if (retval == null)
        {
            retval = PathManager.Instance.Map.GetStartPlate();
        }

        Plate beforePlate = null;

        for (int i = 0; i < amount + _moveCoefficient; i++)
        {
            if (retval == null) return null;

            retval.NextPlate(beforePlate, (plate) =>
            {
                beforePlate = retval;
                retval = plate;
            });

        }

        return retval;
    }

    public override void Move(int amount)
    {
        OffFocus();
        BoardManager.Instance.ResetPawnAt(_nowPlate);
        base.Move(amount + _moveCoefficient);
    }

    protected override void GoStartPlate(Action callback)
    {
        _owner.LeavePawn(Id);
        base.GoStartPlate(callback);
    }

    public override void ArrivePlate(Plate value)
    {
        base.ArrivePlate(value);
        BoardManager.Instance.OverlapProcess(value, this);
        BoardManager.Instance.AbilityEvent(value, this, () =>
            {
                GetCC().EndTurn();
            });
    }

    public override void ArriveGoal()
    {
        base.ArriveGoal();

        _statAdder.LevelUp(_targetCC);

    }

    public IPlayableCharacter GetCharacterController()
    {
        return _targetCC;
    }

    public void AddMoney(int amount)
    {
        _statAdder.AddMoney(_targetCC, amount);
    }

    public void AddAttack(int attackAmount)
    {
        if (_piggyBacking != null)
            _piggyBacking.AddAttack(attackAmount);

        _statAdder.AddAttack(_targetCC, attackAmount);
    }

    public void AddDefence(int defenceAmount)
    {
        if (_piggyBacking != null)
            _piggyBacking.AddDefence(defenceAmount);

        _statAdder.AddDefence(_targetCC, defenceAmount);
    }

    public void AddHealth(int healAmount)
    {
        if (_piggyBacking != null)
            _piggyBacking.AddHealth(healAmount);

        _statAdder.AddHealth(_targetCC, healAmount);
    }

    public void PiggyBack(GamePawn target)
    {
        if (_piggyBacking)
        {
            _piggyBacking.PiggyBack(target);
            return;
        }
        _piggyBacking = target;
        target.PiggyBacked();
    }

    protected void PiggyBacked()
    {
        _nowPlate = null;
        _isPiggyBacked = true;
    }

    public void EnterTurn()
    {
        if (_isPiggyBacked) return;
        _select.SetSelectable();
    }

    public void ExitTurn()
    {
        if (_isPiggyBacked) return;
        _select.SetUnselectable();
    }

    public void OnFocus()
    {
        _select.OnSelectAction();
    }

    public void OffFocus()
    {
        _select.OffSelectAction();
    }
    
    public virtual void Killed()
    {
        BackHome();

        SoundManager.Instance.PlaySFX("Kill");
        //Instantiate(_catchEffect, transform.position + Vector3.up, Quaternion.identity);
    }

    public virtual void BackHome()
    {
        _statAdder.Reset();
        _owner.BackHomePawn(Id);
    }

    public int Value()
    {
        if (_isPiggyBacked) return 0;

        int ret = 1;

        GamePawn target = _piggyBacking;

        while (target != null)
        {
            ret++;
        }

        return ret;
    }
}