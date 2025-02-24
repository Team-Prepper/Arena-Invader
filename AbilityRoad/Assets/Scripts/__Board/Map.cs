using UnityEngine;

namespace BoardGame
{
    public class Map : MonoBehaviour
    {

        [SerializeField] Plate _startPlate;

        public Plate GetStartPlate()
        {
            return _startPlate;
        }

    }
    
}