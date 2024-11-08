using System;
using System.Collections;
using UnityEngine;

public class Pawn : MonoBehaviour {

    [SerializeField] Player _owner;
    [SerializeField] Transform _model;
    [SerializeField] IPawnMove _moveOnMap;
    [SerializeField] IMoveTo _moveTo;

    Pawn _piggyBacking;
    bool _isPiggyBacked;

    [SerializeField] IPlate _nowPlate;

    [SerializeField] Vector3 _up = Vector3.up;

    int _movePoint = 0;

    [SerializeField] int _attackCoefficient = 1;
    [SerializeField] int _defenseCoefficient = 1;
    [SerializeField] int _healCoefficient = 1;
    [SerializeField] int _moveCoefficient = 0;
    [SerializeField] int _levelCoefficient = 1;

    public void SetOwner(Player player)
    {
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        _owner = player;
    }

    public Player GetOwner()
    {
        return _owner;
    }

    public void SetColor(Color color) {
        _model.GetComponent<Renderer>().material.color = color;

    }

    public IPlate MovePredict(int amount) {

        IPlate plate = _nowPlate;

        if (plate == null)
        {
            plate = GameManager.Instance.Playground.Map.GetStartPlate();
        }

        return _moveOnMap.Predict(plate, this, amount);

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

        _nowPlate = null;
        _isPiggyBacked = false;
        GetOwner().BackHomePawn(this);
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

    public void AddAttack(int attackAmount)
    {
        if (_piggyBacking != null) _piggyBacking.AddAttack(attackAmount);
        GetOwner().AddAttack(attackAmount * _attackCoefficient);
    }

    public void AddDefence(int defenceAmount)
    {
        if (_piggyBacking != null) _piggyBacking.AddDefence(defenceAmount);
        GetOwner().AddDefence(defenceAmount * _defenseCoefficient);
    }

    public void AddHeal(int healAmount)
    {
        if (_piggyBacking != null) _piggyBacking.AddHeal(healAmount);
        GetOwner().AddHeal(healAmount * _healCoefficient);
    }
}