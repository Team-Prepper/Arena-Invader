using UnityEngine;

public class Map : MonoBehaviour {

    [SerializeField] IPlate _startPlate;

    public IPlate GetStartPlate() {
        return _startPlate;
    }

}