using System;
using UnityEngine;

namespace BoardGame
{

    public class Plate : MonoBehaviour
    {

        private static readonly IOverlapEvent _sameOwnerOverlapEvent = new PiggyBack();
        private static readonly IOverlapEvent _otherOwnerOverlapEvent = new OneMoreAndBackHome();

        [SerializeField] private INextPlateSelector _nextPlateSelector;

        private Pawn _nowPawn;

        public void Arrive(Pawn pawn, Action callback = null)
        {
            if (!_nowPawn)
            {
                SetPawn(pawn);
                PlayEvent();
                return;
            }

            if (pawn.GetOwner() == _nowPawn.GetOwner())
            {
                _sameOwnerOverlapEvent?.Event(this, pawn);
                PlayEvent();
                return;
            }

            _otherOwnerOverlapEvent?.Event(this, pawn);

            Invoke(nameof(PlayEvent), 1f);

        }

        protected virtual void PlayEvent()
        {
            _nowPawn.GetPlayer().EndTurn();
        }

        public void Leave(Pawn target, Action<Plate> callback)
        {
            SetPawn(null);
            callback?.Invoke(_nextPlateSelector.NextPlate(null));
        }

        public void NextPlate(Plate from, Action<Plate> callback)
        {
            callback?.Invoke(_nextPlateSelector.NextPlate(from));
        }

        public virtual int GetValue()
        {
            return _nextPlateSelector.GetValue();
        }

        public void SetPawn(Pawn target)
        {
            _nowPawn = target;
        }

        public Pawn GetPawn() {
            return _nowPawn;
        }

    }
}