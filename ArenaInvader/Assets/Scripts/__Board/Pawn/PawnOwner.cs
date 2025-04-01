using BoardGame;
using UnityEngine;
using System.Collections.Generic;

namespace BoardGame
{

    public class PawnOwner : MonoBehaviour
    {

        [SerializeField] public IList<Pawn> _pawns;
        [SerializeField] Transform[] _pawnPosition;
        [SerializeField] Color[] _pawnColor;

        IList<Vector3> _emptyPlace;

        public void SetPawn(IList<Pawn> pawns)
        {
            _pawns = pawns;
        }

        public void SetInitial(IBoardPlayer player, int idx)
        {
            for (int i = 0; i < _pawns.Count; i++)
            {
                _pawns[i].SetOwner(player, this, i);

                _pawns[i].SetColor(_pawnColor[idx]);
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
            foreach (Pawn pawn in _pawns)
            {
                pawn.EnterTurn();
            }

        }

        public void OffPawnChoose()
        {
            foreach (Pawn pawn in _pawns)
            {
                pawn.ExitTurn();
            }
        }
    }
}