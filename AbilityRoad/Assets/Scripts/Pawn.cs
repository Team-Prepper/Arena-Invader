using System;
using Unity.VisualScripting;
using UnityEngine;

public class Pawn : MonoBehaviour {

    [SerializeField] Player _owner;
    [SerializeField] Transform _model;

    [SerializeField] IPlate _nowPlate;
    [SerializeField] IPlate _beforePlate;

    [SerializeField] Vector3 _up = Vector3.up;

    int _movePoint = 0;

    internal void SetOwner(Player player)
    {
        _owner = player;
    }

    public Player GetOwner() {
        return _owner;
    }

    public void SetColor(Color color) {
        _model.GetComponent<Renderer>().material.color = color;

    }

    public void Move(int amount)
    {
        _movePoint = amount;

        if (_nowPlate == null)
        {
            _owner.LeavePawn(this);
            _nowPlate = GameManager.Instance.Playground.Map.GetStartPlate();

        }

        _nowPlate.Leave(MoveTo);

    }

    public void MoveTo(IPlate plate)
    {

        if (plate == null) {
            _nowPlate = null;
            _owner.LevelUp(this);
            return;
        }

        _movePoint--;
        transform.position = plate.transform.position;

        _beforePlate = _nowPlate;
        _nowPlate = plate;

        if (_movePoint < 1)
        {
            _nowPlate.Arrive(this);
            return;
        }

        plate.NextPlate(_beforePlate, MoveTo);

    }

    public void EnterTurn()
    {
        transform.position += _up;

    }

    public void ExitTurn()
    {
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

    public bool IsOnMap() {
        return true;
    }
}