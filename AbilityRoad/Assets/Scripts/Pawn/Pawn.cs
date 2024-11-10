using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Pawn : MonoBehaviour {

    [SerializeField] SpriteRenderer _sprite;
    [SerializeField] BasePlayer _owner;
    [SerializeField] Transform _model;
    [SerializeField] IPawnMove _moveOnMap;
    [SerializeField] IMoveTo _moveTo;
    
    private int _attackValue = 0;
    private int _defenceValue = 0;
    private int _healthValue = 0;

    Pawn _piggyBacking;
    bool _isPiggyBacked = false;

    [SerializeField] IPlate _nowPlate;

    [SerializeField] Vector3 _up = Vector3.up;

    int _movePoint = 0;

    [SerializeField] int _attackCoefficient = 1;
    [SerializeField] int _defenseCoefficient = 1;
    [SerializeField] int _healthCoefficient = 1;
    [SerializeField] int _moneyCoefficient = 1;
    [SerializeField] int _moveCoefficient = 0;
    [SerializeField] int _levelCoefficient = 1;

    public void SetOwner(BasePlayer player)
    {
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        _owner = player;
    }

    public BasePlayer GetOwner()
    {
        return _owner;
    }

    public void SetColor(Color color) {
        _sprite.color = color;
    }

    public IPlate MovePredict(int amount) {

        IPlate plate = _nowPlate;

        if (plate == null)
        {
            plate = GameManager.Instance.Playground.Map.GetStartPlate();
        }

        return _moveOnMap.Predict(plate, this, amount + _moveCoefficient);

    }

    public void Move(int amount)
    {
        _movePoint = amount + _moveCoefficient;

        void PawnMove() {
            _moveOnMap.MoveTo(_nowPlate, this, _movePoint, Arrive, (value) => {
                _nowPlate = value;
                _nowPlate.Arrive(this, () => { GetOwner().EndTurn(); });
            });
        }

        if (_nowPlate != null)
        {
            PawnMove();
            return;
        }

        _owner.LeavePawn(this);
        _nowPlate = GameManager.Instance.Playground.Map.GetStartPlate();

        _moveTo.MoveTo(_nowPlate.transform.position, () =>
        {
            PawnMove();
        });

    }

    public void Dispose(Vector3 pos) {
        if (!_owner.IsAlive()) return;
        _moveTo.MoveTo(pos, null);
    }

    void Arrive()
    {

        if (_piggyBacking)
        {
            _piggyBacking.transform.SetParent(null);
            _piggyBacking.Arrive();
            _piggyBacking = null;
        }

        _nowPlate = null;
        _isPiggyBacked = false;
        GetOwner().AddAttack(_attackValue);
        GetOwner().AddDefence(_defenceValue);
        GetOwner().AddHeal(_healthValue);
        GetOwner().LevelUp(this, _levelCoefficient, _isPiggyBacked);

    }

    public void BackHome()
    {

        if (_piggyBacking)
        {
            _piggyBacking.transform.SetParent(null);
            _piggyBacking.BackHome();
            _piggyBacking = null;
        }

        ResetImproveState();

        if (_nowPlate == null)
        {
            if (_isPiggyBacked) {
                _isPiggyBacked = false;
                GetOwner().BackHomePawn(this);
            }
            return;
        }
        _isPiggyBacked = false;
        _nowPlate.SetPawn(null);
        _nowPlate = null;
        GetOwner().BackHomePawn(this);
    }

    private void ResetImproveState()
    {
        _healthValue = 0;
        _attackValue = 0;
        _defenceValue = 0;
    }

    public void PiggyBack(Pawn target)
    {
        if (_piggyBacking)
        {
            _piggyBacking.PiggyBack(target);
            return;
        }
        _piggyBacking = target;
        target.PiggyBacked(this);
    }

    protected void PiggyBacked(Pawn owner)
    {
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        _nowPlate = null;
        _isPiggyBacked = true;
        transform.SetParent(owner._model.transform);
        transform.position = owner.transform.position + _up;
    }

    public void EnterTurn()
    {
        if (_isPiggyBacked) return;

        gameObject.layer = LayerMask.NameToLayer("Default");
        transform.position += _up;
    }

    public void ExitTurn()
    {
        if (_isPiggyBacked) return;

        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        transform.position -= _up;

    }

    public void OnFocus()
    {
        _model.position += _up;
    }

    public void OffFocus()
    {
        _model.position -= _up;
    }

    public void AddMoney(int amount) {
        GetOwner().AddMoney(amount * _moneyCoefficient);
    }

    public void AddAttack(int attackAmount)
    {
        if (_piggyBacking != null) _piggyBacking.AddAttack(attackAmount);
        _attackValue += attackAmount * _attackCoefficient;
    }

    public void AddDefence(int defenceAmount)
    {
        if (_piggyBacking != null) _piggyBacking.AddDefence(defenceAmount);
        _defenceValue += defenceAmount * _defenseCoefficient;
    }

    public void AddHealth(int healAmount)
    {
        if (_piggyBacking != null) _piggyBacking.AddHealth(healAmount);
        _healthValue += healAmount * _healthCoefficient;
    }
    
    public bool IsPiggyBacked()
    {
        return _isPiggyBacked;
    }
    
    public bool isPiggied()
    {
        return _piggyBacking != null;
    }
}