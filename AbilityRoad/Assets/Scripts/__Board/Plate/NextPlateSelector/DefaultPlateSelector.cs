using UnityEngine;

namespace BoardGame
{
    public class DefaultPlateSelector : INextPlateSelector
    {

        [SerializeField] Plate _nextPlate;

        public override Plate NextPlate(Plate from)
        {
            return _nextPlate;
        }

    }
}