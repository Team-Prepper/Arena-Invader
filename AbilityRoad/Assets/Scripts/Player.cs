using EHTool.UIKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] protected  Pawn[] _pawns;
    [SerializeField] Transform[] _pawnPosition;
    [SerializeField] Color[] _pawnColor;

    IList<Vector3> _emptyPlace;

    [SerializeField] IGUIUnitHealth _healthUI;
    [SerializeField] Status _status;
    [SerializeField] int _health;

    int _level;
    protected int _chance = 0;

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

        _healthUI.SetHealth(_health);

    }

    internal void AddHeal(int healAmount)
    {
        _health += healAmount;
        _healthUI.SetHealth(_health);
    }

    public void RedueHealth(int amount)
    {
        _health -= amount;
        _healthUI.SetHealth(_health);

        if (_health >= 0) return;

        // �׾��� �� �̺�Ʈ ó��
    }

    internal void AddAttack(int attackAmount)
    {
        _status.AddAttackValue(attackAmount);
    }

    internal void AddDefence(int defenceAmount)
    {
        _status.AddDefenceValue(defenceAmount);
    }

    public void LeavePawn(Pawn pawn) {
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

    protected virtual void RollDice()
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