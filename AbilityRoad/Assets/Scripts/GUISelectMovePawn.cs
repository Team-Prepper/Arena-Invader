using EHTool.UIKit;
using UnityEngine;
using UnityEngine.UI;

public class GUISelectMovePawn : GUIPopUp {

    [SerializeField] Text _amountTxt;
    [SerializeField] GameObject _pawnPredict;

    BasePlayer _target;
    Pawn _selectedPawn;
    int _amount;

    public void SetPlayer(BasePlayer target, int amount)
    {
        _target = target;
        _amount = amount;
        _target.OnPawnChoose();

        _amountTxt.text = amount.ToString();
        _pawnPredict.transform.SetParent(null);
        _pawnPredict.transform.localScale = Vector3.one;
        _pawnPredict.transform.SetParent(transform);
        _pawnPredict.SetActive(false);
    }

    private void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit))
        {
            CleanUp();
            return;
        }

        if (!hit.transform.CompareTag("Pawn"))
        {
            CleanUp();
            return;
        }

        Pawn newPawn = hit.transform.GetComponent<Pawn>();
        
        if (newPawn == null) return;
        
        if (newPawn.GetOwner() != _target)
        {
            CleanUp();
            return;
        }

        if (_selectedPawn != newPawn)
        {
            _selectedPawn?.OffFocus();
            _selectedPawn = newPawn;
            if (newPawn != null)
            {
                newPawn.OnFocus();
                IPlate nextPlate = newPawn.MovePredict(_amount);
                if (nextPlate != null)
                {
                    _pawnPredict.transform.position = nextPlate.transform.position;
                    _pawnPredict.SetActive(true);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _target.OffPawnChoose();
            _selectedPawn.OffFocus();
            _selectedPawn.Move(_amount);
            Close();
        }

    }

    void CleanUp()
    {
        if (_selectedPawn == null) return;

        _selectedPawn.OffFocus();
        _selectedPawn = null;
        _pawnPredict.SetActive(false);
    }
}