using BoardGame;
using UnityEngine;
using UnityEngine.UI;

public class GUISelectMovePawn : GUINetworkPopUp<int> {

    [SerializeField] private Text _amountTxt;
    [SerializeField] private Text _addedAmountTxt;
    [SerializeField] private GameObject _pawnPredict;

    private ICharacterController _target;
    private Pawn _selectedPawn;
    private int _amount;

    public void SetPlayer(ICharacterController target, int amount, int addedAmount)
    {
        _target = target;
        _amount = amount + addedAmount;

        _amountTxt.text = amount.ToString();

        if (addedAmount > 0) {
            _addedAmountTxt.text = string.Format("+{0}", addedAmount);
        }
        else if (addedAmount < 0) {
            _addedAmountTxt.text = string.Format("{0}", addedAmount);
        }
        else {
            _addedAmountTxt.text = "";
        }

        _pawnPredict.transform.SetParent(null);
        _pawnPredict.transform.localScale = Vector3.one;
        _pawnPredict.transform.SetParent(transform);
        _pawnPredict.SetActive(false);
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

        
        if (!hit.transform.TryGetComponent(out GamePawn newPawn)) {
            return null;
        }

        if (newPawn.GetPlayer() != _target)
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
            int idx = newPawn.Id;

            FocusPawn(idx);
            NetworkModify(idx);
        }

        if (Input.GetMouseButtonUp(0))
        {
            MovePawn(_selectedPawn.Id);
        }

    }

    public override void NetworkModifiedEvent(int value) {
        if (IsControlled) return;
        FocusPawn(value);
    }

    public void FocusPawn(int value)
    {

        if (_selectedPawn != null)
        {
            _selectedPawn.OffFocus();
            _selectedPawn = null;
            _pawnPredict.SetActive(false);

        }

        if (value < 0) return;

        _selectedPawn = _target.Target.Pawns[value];
        _selectedPawn.OnFocus();

        Plate nextPlate = _selectedPawn.MovePredict(_amount);

        if (nextPlate != null)
        {
            _pawnPredict.transform.position = nextPlate.transform.position;
            _pawnPredict.SetActive(true);
        }

    }

    public void MovePawn(int idx) {
        _pawnPredict.SetActive(false);
        _target.MovePawn(idx, _amount);
        TryClose();

    }

}