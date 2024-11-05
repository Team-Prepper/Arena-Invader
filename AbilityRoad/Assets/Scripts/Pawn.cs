using System;
using System.Collections;
using UnityEngine;

public class Pawn : MonoBehaviour {

    [SerializeField] Player _owner;
    [SerializeField] Transform _model;
    [SerializeField] IPawnMove _move;

    Pawn _piggyBacking;
    bool _isPiggyBacked;

    [SerializeField] IPlate _nowPlate;
    [SerializeField] IPlate _beforePlate;

    [SerializeField] Vector3 _up = Vector3.up;

    [SerializeField] float _moveTime = 0.5f;
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

    public void Move(int amount)
    {
        _movePoint = amount + _moveCoefficient;

        if (_nowPlate == null)
        {
            _owner.LeavePawn(this);
            _nowPlate = GameManager.Instance.Playground.Map.GetStartPlate();

            _move.MoveTo(_nowPlate.transform.position, _moveTime, 0f, () => {
                _nowPlate.Leave(this, MoveTo);
            });

            return;
        }

        _nowPlate.Leave(this, MoveTo);

    }

    public void MoveTo(Vector3 pos) {
        _move.MoveTo(pos, _moveTime, 0, null);
    }

    public void MoveTo(IPlate plate)
    {
        if (plate == null)
        {
            Arrive();
            return;
        }

        _movePoint--;

        _move.MoveTo(plate.transform.position, _moveTime, 0.1f, () => {
            _beforePlate = _nowPlate;
            _nowPlate = plate;

            if (_movePoint < 1)
            {
                _nowPlate.Arrive(this, () => { GetOwner().EndTurn(); });
                return;
            }

            plate.NextPlate(_beforePlate, MoveTo);

        });
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
        _owner.LevelUp(this, _levelCoefficient, _isPiggyBacked);
        _isPiggyBacked = false;

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
        GetOwner().BackHomePawn(this);
        _isPiggyBacked = false;
    }

    public void PiggyBack(Pawn target)
    {
        if (_piggyBacking)
        {
            _piggyBacking.PiggyBack(target);
            return;
        }
        target.PiggyBacked(this);
        _piggyBacking = target;
    }

    protected void PiggyBacked(Pawn owner) {
        _nowPlate = null;
        _isPiggyBacked = true;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
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