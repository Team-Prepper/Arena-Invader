using EHTool.UIKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] Pawn[] _pawns;
    [SerializeField] Transform[] _pawnPosition;
    [SerializeField] Color[] _pawnColor;

    IList<Vector3> _emptyPlace;

    int _level;

    [SerializeField] int _attack;
    [SerializeField] int _defense;
    [SerializeField] int _health;

    int _chance = 0;

    public void SetInitial(int idx)
    {
        _level = 0;

        GameManager.Instance.Playground.AddPlayer(this);

        for (int i = 0; i < _pawns.Length; i++) {
            _pawns[i].SetOwner(this);
            _pawns[i].SetColor(_pawnColor[idx]);
            _pawns[i].transform.position = _pawnPosition[i].position;
        }

        _emptyPlace = new List<Vector3>();

    }

    internal void AddHeal(int healAmount)
    {
        _health += healAmount;
    }

    internal void AddAttack(int attackAmount)
    {
        _attack += attackAmount;
    }

    internal void AddDefence(int defenceAmount)
    {
        _defense += defenceAmount;
    }

    public void LeavePawn(Pawn pawn) {
        _emptyPlace.Add(pawn.transform.position);
    }

    public void LevelUp(Pawn pawn, int levelUpAmount, bool isPiggyBacked)
    {
        _level += levelUpAmount;

        pawn.MoveTo(_emptyPlace[0]);
        _emptyPlace.RemoveAt(0);

        if (isPiggyBacked) return;

        EndTurn();
    }

    public void BackHomePawn(Pawn pawn)
    {
        pawn.MoveTo(_emptyPlace[0]);
        _emptyPlace.RemoveAt(0);

    }

    public void StartTurn()
    {
        _chance = 1;
        RollDice();
    }

    public void EndTurn() {
        if (_chance == 0)
        {
            GameManager.Instance.Playground.TurnEnd();
            return;
        }
        RollDice();
    }

    public void AddChance() {
        _chance++;
    }

    void RollDice()
    {
        _chance--;

        UIManager.Instance.OpenGUI<GUIDice>("Dice").SetCallback((value) => {
            UIManager.Instance.OpenGUI<GUISelectMovePawn>("SelectMovePawn").SetPlayer(this, value);
        });

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