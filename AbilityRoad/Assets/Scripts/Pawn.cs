using System;
using System.Collections;
using UnityEngine;

public class Pawn : MonoBehaviour {

    [SerializeField] Player _owner;
    [SerializeField] Transform _model;

    Pawn _piggyBacked;
    bool _isPiggyBacked;

    [SerializeField] IPlate _nowPlate;
    [SerializeField] IPlate _beforePlate;

    [SerializeField] Vector3 _up = Vector3.up;

    [SerializeField] float _moveTime = 0.5f;
    int _movePoint = 0;

    internal void SetOwner(Player player)
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
        _movePoint = amount;

        if (_nowPlate == null)
        {
            _owner.LeavePawn(this);
            _nowPlate = GameManager.Instance.Playground.Map.GetStartPlate();

            StartCoroutine(_MoveTo(_nowPlate.transform.position, _moveTime, 0f, () => {
                _nowPlate.Leave(MoveTo);
            }));

            return;
        }

        _nowPlate.Leave(MoveTo);

    }

    public void MoveTo(IPlate plate)
    {
        if (plate == null)
        {
            Arrive();
            return;
        }

        _movePoint--;

        StartCoroutine(_MoveTo(plate.transform.position, _moveTime, 0.1f, () => {
            _beforePlate = _nowPlate;
            _nowPlate = plate;

            if (_movePoint < 1)
            {
                _nowPlate.Arrive(this);
                return;
            }

            plate.NextPlate(_beforePlate, MoveTo);

        }));
    }

    void Arrive()
    {

        if (_piggyBacked)
        {
            _piggyBacked.transform.SetParent(null);
            _piggyBacked.Arrive();
        }

        _nowPlate = null;
        _owner.LevelUp(this);
        _isPiggyBacked = false;

    }

    IEnumerator _MoveTo(Vector3 goalPos, float moveTime, float stopTime, CallbackMethod callback)
    {
        float spendTime = 0;
        Vector3 originPos = transform.position;
        while (spendTime < moveTime) {
            yield return null;
            spendTime += Time.deltaTime;
            transform.position = Vector3.Lerp(originPos, goalPos, spendTime / moveTime);
        }

        yield return new WaitForSeconds(stopTime);

        transform.position = goalPos;
        callback?.Invoke();
    }

    public void PiggyBack(Pawn target) {
        target.PiggyBacked(this);
        _piggyBacked = target;
    }

    protected void PiggyBacked(Pawn owner) {
        transform.position += _up;
        _nowPlate = null;
        _isPiggyBacked = true;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        transform.SetParent(owner._model.transform);
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

    public bool IsOnMap() {
        return true;
    }
}