using System;
using System.Collections;
using UnityEngine;

public class DartDice : IDice {

    bool _beforeHit = false;

    [SerializeField] Transform _plateTr;
    [SerializeField] Transform _dartPoint;
    [SerializeField] float _rotateSpeed;
    [SerializeField] float _stopTime;

    public override void Initial()
    {

    }

    private void Update()
    {
        if (!_beforeHit) return;

        transform.Rotate(_rotateSpeed * Time.deltaTime * Vector3.forward);
    }

    public int GetValue() {
        return 1;
    }

    public override void Roll(CallbackMethod<int> callback)
    {
        _beforeHit = false;
        _dartPoint.SetParent(transform);
    }

    IEnumerator Stop(CallbackMethod<int> callback) {
        float spendTime = 0;

        while (spendTime < _stopTime) {
            yield return null;

            transform.Rotate(Mathf.Lerp(_rotateSpeed, 0, spendTime / _stopTime) * Time.deltaTime * Vector3.forward);
            spendTime += Time.deltaTime;

        }

        callback?.Invoke(GetValue());
    }


}
