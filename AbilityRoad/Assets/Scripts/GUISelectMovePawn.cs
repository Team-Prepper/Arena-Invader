using EHTool.UIKit;
using UnityEngine;

public class GUISelectMovePawn : GUIPopUp {

    Player _target;
    Pawn _selectedPawn;
    int _amount;

    public void SetPlayer(Player target, int amount) {
        _target = target;
        _amount = amount;
        _target.OnPawnChoose();
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

        if (_selectedPawn != newPawn) {
            _selectedPawn?.OffFocus();
            newPawn?.OnFocus();
            _selectedPawn = newPawn;
        }

        if (Input.GetMouseButtonUp(0)) {
            _target.OffPawnChoose();
            _selectedPawn.OffFocus();
            _selectedPawn.Move(_amount);
            Close();
        }

        
    }

    void CleanUp() {
        _selectedPawn?.OffFocus();
        _selectedPawn = null;
    }
}