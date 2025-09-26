using UnityEngine;
using System.Collections.Generic;


public class PawnOwner : MonoBehaviour, IPawnOwner
{

    [SerializeField] private Transform[] _pawnPosition;

    public IList<GamePawn> _pawns;
    public IList<GamePawn> Pawns => _pawns;

    private IList<Vector3> _emptyPlace;
    private IPlayableCharacter _cc;

    public void SetCC(IPlayableCharacter cc)
    {
        _cc = cc;
    }

    public void SetInitial(string characterCode)
    {
        BasePlayer Target = CharacterManager.Instance.
            SpawnPlayer(characterCode).GetComponent<BasePlayer>();

        Target.transform.SetParent(transform);
        Target.transform.localPosition = Vector3.zero;

        _pawns = Target.Pawns;

        for (int i = 0; i < _pawns.Count; i++)
        {
            _pawns[i].SetTargetCC(_cc);
            _pawns[i].SetOwner(this);
            _pawns[i].Id = i;

            _pawns[i].transform.position = _pawnPosition[i].position;
        }

        _emptyPlace = new List<Vector3>();

    }

    public void ResetPawn()
    {
        foreach (var p in _pawns)
        {
            p.BackHome();
        }
    }

    public void MovePawn(int pawnId, int amount)
    {
        OffPawnChoose();
        _pawns[pawnId].Move(amount);
    }

    public void LeavePawn(int id)
    {
        _emptyPlace.Add(_pawns[id].transform.position);
    }

    public void BackHomePawn(int id)
    {
        _pawns[id].Dispose(_emptyPlace[0]);
        _emptyPlace.RemoveAt(0);

    }

    public void OnPawnChoose()
    {
        foreach (GamePawn pawn in _pawns)
        {
            pawn.EnterTurn();
        }

    }

    public void OffPawnChoose()
    {
        foreach (GamePawn pawn in _pawns)
        {
            pawn.ExitTurn();
        }
    }
}