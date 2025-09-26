using UnityEngine;


public class AbovePawnSelect : PawnSelectBase
{

    [SerializeField] private Transform _model;
    [SerializeField] private Vector3 _up = Vector3.up;

    public override void Initial()
    {
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    public override void SetSelectable()
    {

        gameObject.layer = LayerMask.NameToLayer("Default");
        transform.position += _up;

    }

    public override void SetUnselectable()
    {
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        transform.position -= _up;
    }

    public override void OnSelectAction()
    {
        _model.position += _up;
    }

    public override void OffSelectAction()
    {
        _model.position -= _up;
    }

    public override void PiggyBackAction(GamePawn owner)
    {
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        transform.SetParent(owner.transform);
        transform.position = owner.transform.position + _up;
    }

    public override void ResetAction()
    {

    }

}