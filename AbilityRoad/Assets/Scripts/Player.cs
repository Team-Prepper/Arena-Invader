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
    int _housePawnCnt;

    public void SetInitial(int idx)
    {
        _level = 0;

        GameManager.Instance.Playground.AddPlayer(this);

        for (int i = 0; i < _pawns.Length; i++) {
            _pawns[i].SetOwner(this);
            _pawns[i].SetColor(_pawnColor[idx]);
            _pawns[i].transform.position = _pawnPosition[i].position;
        }

        _housePawnCnt = _pawns.Length;
        _emptyPlace = new List<Vector3>();

    }

    public void LeavePawn(Pawn pawn) {
        _emptyPlace.Add(pawn.transform.position);
    }

    public void LevelUp(Pawn pawn)
    {
        _level++;

        pawn.transform.position = _emptyPlace[0];
        _emptyPlace.RemoveAt(0);

        EndTurn();
    }

    public void StartTurn()
    {
        UIManager.Instance.OpenGUI<GUIDice>("Dice").SetCallback((value) => {
            UIManager.Instance.OpenGUI<GUISelectMovePawn>("SelectMovePawn").SetPlayer(this, value);
        });

    }

    public void EndTurn() {
        GameManager.Instance.Playground.TurnEnd();
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