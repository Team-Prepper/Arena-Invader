using UnityEngine;


public abstract class PawnSelectBase : MonoBehaviour
{

    public abstract void Initial();
    public abstract void SetSelectable();
    public abstract void SetUnselectable();

    public abstract void OnSelectAction();
    public abstract void OffSelectAction();

    public abstract void PiggyBackAction(GamePawn owner);
    public abstract void ResetAction();

}