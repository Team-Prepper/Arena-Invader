using EHTool.UIKit;
using UnityEngine;
using UnityEngine.UI;

public class GUISelectMovePawn : GUIPopUp {

    [SerializeField] Text _amountTxt;

    Player _target;
    Pawn _selectedPawn;
    int _amount;

    public void SetPlayer(Player target, int amount)
    {
        _target = target;
        _amount = amount;
        _target.OnPawnChoose();

        _amountTxt.text = amount.ToString();
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

        if (newPawn.GetOwner() != _target)
        {
            CleanUp();
            return;
        }

        if (_selectedPawn != newPawn)
        {
            _selectedPawn?.OffFocus();
            newPawn?.OnFocus();
            _selectedPawn = newPawn;
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
        _selectedPawn?.OffFocus();
        _selectedPawn = null;
    }
}