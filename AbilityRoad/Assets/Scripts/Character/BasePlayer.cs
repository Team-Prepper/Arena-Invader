using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayer : Character
{
    [SerializeField] protected Pawn[] _pawns;
    [SerializeField] Transform[] _pawnPosition;
    [SerializeField] Color[] _pawnColor;

    IList<Vector3> _emptyPlace;

    protected int _chance = 0;

    protected override void DeathEvent()
    {
        base.DeathEvent();
        GameManager.Instance.Playground.PlayerDeath(this);
    }

    public void SetInitial(int idx, string name)
    {
        _level = 0;
        _name = name;

        GameManager.Instance.Playground.AddPlayer(this);
        for (int i = 0; i < _pawns.Length; i++)
        {
            _pawns[i].SetOwner(this);

            _pawns[i].SetColor(_pawnColor[idx]);
            _pawns[i].transform.position = _pawnPosition[i].position;
        }

        _emptyPlace = new List<Vector3>();

        _healthUI.SetHealth(_health);

    }

    public void LeavePawn(Pawn pawn)
    {
        _emptyPlace.Add(pawn.transform.position);
    }

    public void LevelUp(Pawn pawn, int levelUpAmount, bool isPiggyBacked)
    {
        _level += levelUpAmount;

        pawn.Dispose(_emptyPlace[0]);
        _emptyPlace.RemoveAt(0);

        if (isPiggyBacked) return;

        EndTurn();
    }

    public void BackHomePawn(Pawn pawn)
    {
        pawn.Dispose(_emptyPlace[0]);
        _emptyPlace.RemoveAt(0);

    }

    public void StartTurn()
    {
        _chance = 1;
        RollDice();
    }

    public void EndTurn()
    {
        if (_chance == 0)
        {
            GameManager.Instance.Playground.TurnEnd();
            return;
        }
        RollDice();
    }

    public void AddChance()
    {
        _chance++;
    }

    protected virtual void RollDice()
    {

    }

    public void OnPawnChoose()
    {
        for (int i = 0; i < _pawns.Length; i++)
        {
            _pawns[i].EnterTurn();
        }

    }
    public void OffPawnChoose()
    {
        for (int i = 0; i < _pawns.Length; i++)
        {
            _pawns[i].ExitTurn();
        }

    }
}