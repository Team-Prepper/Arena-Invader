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
        GameObject spawnedPlayer = CharacterManager.Instance.SpawnPlayer(characterCode);
        BasePlayer target = spawnedPlayer.GetComponent<BasePlayer>();
        if (target == null)
        {
            throw new MissingComponentException(
                $"Spawned character '{characterCode}' does not have BasePlayer.");
        }

        target.transform.SetParent(transform);
        target.transform.localPosition = Vector3.zero;

        _pawns = target.Pawns;
        _emptyPlace = new List<Vector3>(_pawns.Count);

        for (int i = 0; i < _pawns.Count; i++)
        {
            if (i >= _pawnPosition.Length)
            {
                throw new System.IndexOutOfRangeException(
                    $"{name} has fewer pawn positions than spawned pawns.");
            }

            _pawns[i].SetTargetCC(_cc);
            _pawns[i].SetOwner(this);
            _pawns[i].Id = i;

            _pawns[i].transform.position = _pawnPosition[i].position;
        }

    }

    public void ResetPawn()
    {
        if (_pawns == null) return;
        foreach (var p in _pawns)
        {
            p.BackHome();
        }
    }

    public void MovePawn(int pawnId, int amount)
    {
        if (_pawns == null) return;
        if (pawnId < 0 || pawnId >= _pawns.Count) return;

        OffPawnChoose();
        _pawns[pawnId].Move(amount);
    }

    public void LeavePawn(int id)
    {
        if (_pawns == null) return;
        if (id < 0 || id >= _pawns.Count) return;

        _emptyPlace.Add(_pawns[id].transform.position);
    }

    public void BackHomePawn(int id)
    {
        if (_pawns == null) return;
        if (id < 0 || id >= _pawns.Count) return;
        if (_emptyPlace == null || _emptyPlace.Count == 0) return;

        Vector3 targetPosition = _emptyPlace[0];
        _emptyPlace.RemoveAt(0);
        _pawns[id].Dispose(targetPosition);

    }

    public void OnPawnChoose()
    {
        if (_pawns == null) return;
        foreach (GamePawn pawn in _pawns)
        {
            pawn.EnterTurn();
        }

    }

    public void OffPawnChoose()
    {
        if (_pawns == null) return;
        foreach (GamePawn pawn in _pawns)
        {
            pawn.ExitTurn();
        }
    }
}
