using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchPlate : IPlate
{

    [SerializeField] IPlate _nextPlate;

    [System.Serializable]
    public class NextPlateInfor {
        [SerializeField] internal IPlate _from;
        [SerializeField] internal IPlate _to;
    }

    [SerializeField] NextPlateInfor[] _infor;

    IDictionary<IPlate, IPlate> _fromTo;

    private void Start()
    {
        _fromTo = new Dictionary<IPlate, IPlate>();
        for (int i = 0; i < _infor.Length; i++) {
            _fromTo.Add(_infor[i]._from, _infor[i]._to);
        }
    }

    public override void Arrive(Pawn target)
    {
        Debug.Log("´É·Â È¹µæ");
        target.GetOwner().EndTurn();
    }
    public override void Leave(CallbackMethod<IPlate> callback)
    {
        callback?.Invoke(_nextPlate);
    }

    public override void NextPlate(IPlate from, CallbackMethod<IPlate> callback) {
        Debug.Log(from.gameObject.name);
        callback?.Invoke(_fromTo[from]);
    }
}
