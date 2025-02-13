using System;
using UnityEngine;
using UnityEngine.UI;

public class GUISelectMovePawn : GUINetworkPopUp<int> {

    [SerializeField] Text _amountTxt;
    [SerializeField] GameObject _pawnPredict;

    ICharacterController _target;
    Pawn _selectedPawn;
    int _amount;

    public void SetPlayer(ICharacterController target, int amount)
    {
        _target = target;
        _amount = amount;

        _amountTxt.text = amount.ToString();
        _pawnPredict.transform.SetParent(null);
        _pawnPredict.transform.localScale = Vector3.one;
        _pawnPredict.transform.SetParent(transform);
        _pawnPredict.SetActive(false);
    }

    int GetPawnValue(Pawn newPawn) {

        for (int i = 0; i < _target.Target._pawns.Length; i++)
        {
            if (_target.Target._pawns[i] == newPawn)
            {
                return i;
            }
        }
        return -1;
    }

    private Pawn GetPredictPawn()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit))
        {
            return null;
        }

        if (!hit.transform.CompareTag("Pawn"))
        {
            return null;
        }

        
        if (!hit.transform.TryGetComponent(out Pawn newPawn))
            return null;

        if (newPawn.GetOwner() != _target.Target)
        {
            return null;
        }

        return newPawn;

    }

    private void Update()
    {
        if (!IsControlled) return;

        Pawn newPawn = GetPredictPawn();

        if (newPawn == null)
        {
            if (_selectedPawn != null) {
                FocusPawn(-1);
                NetworkModify(-1);
            }
            return;
        }

        if (_selectedPawn != newPawn)
        {
            int idx = GetPawnValue(newPawn);

            FocusPawn(idx);
            NetworkModify(idx);
        }

        if (Input.GetMouseButtonUp(0))
        {
            _target.MovePawn(_selectedPawn.Id, _amount);
            TryClose();
        }

    }

    public override void NetworkModifiedEvent(int value) {
        if (IsControlled) return;
        FocusPawn(value);
    }

    void FocusPawn(int value)
    {
        if (_selectedPawn != null)
        {
            _selectedPawn.OffFocus();
            _selectedPawn = null;
            _pawnPredict.SetActive(false);

        }

        if (value < 0) return;

        _selectedPawn = _target.Target._pawns[value];
        _selectedPawn.OnFocus();

        IPlate nextPlate = _selectedPawn.MovePredict(_amount);

        if (nextPlate != null)
        {
            _pawnPredict.transform.position = nextPlate.transform.position;
            _pawnPredict.SetActive(true);
        }

    }

}