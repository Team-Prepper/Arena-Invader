using UnityEngine;


namespace BoardGame
{

    public abstract class IPawnSelect : MonoBehaviour
    {

        public abstract void Initial();
        public abstract void SetSelectable();
        public abstract void SetUnselectable();

        public abstract void OnSelectAction();
        public abstract void OffSelectAction();

        public abstract void PiggyBackAction(Pawn owner);
        public abstract void ResetAction();

    }
}