using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartDice : IDice {

    bool _beforeHit = false;

    [SerializeField] Transform _plateTr;
    [SerializeField] float _rotateSpeed;

    public override void Initial()
    {

    }

    private void Update()
    {
        if (!_beforeHit) return;
    }

    public override void Roll(CallbackMethod<int> callback)
    {

    }

}
