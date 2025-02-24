using System.Collections.Generic;
using UnityEngine;
using BoardGame;

public class BasePlayer : MonoBehaviour
{
    [SerializeField] private PawnOwner _pawnOwner;
    [SerializeField] private GamePawn[] _pawns;

    public IList<Pawn> Pawns => _pawns;

    private ICharacterController _cc;

    public void SetInitial(ICharacterController cc, int idx)
    {
        _cc = cc;

        for (int i = 0; i < _pawns.Length; i++) {
            _pawns[i].SetOwner(_cc, _pawnOwner, i);
            _pawns[i].SetTargetCC(_cc);
        }

        _pawnOwner.SetPawn(_pawns);
        _pawnOwner.SetInitial(cc, idx);

    }

    public void OnPawnChoose()
    {
        _pawnOwner.OnPawnChoose();
    }

    public void OffPawnChoose()
    {
        _pawnOwner.OffPawnChoose();
    }
    
}