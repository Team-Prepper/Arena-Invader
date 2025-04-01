using UnityEngine;

namespace BoardGame
{

    public abstract class INextPlateSelector : MonoBehaviour
    {
        public abstract Plate NextPlate(Plate from);

    }
}