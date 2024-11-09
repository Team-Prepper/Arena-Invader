using System.Collections;
using UnityEngine;

public class DartDice : IDice {

    bool _beforeHit = false;

    [SerializeField] Transform _plateTr;
    [SerializeField] Transform _dartPoint;
    [SerializeField] float _rotateSpeed;
    [SerializeField] float _stopTime;

    [SerializeField] Transform[] _numTr;
    [SerializeField] int _minValue;

    public override void Initial()
    {
        _plateTr.eulerAngles = Vector3.forward * Random.Range(-180, 180);

    }

    private void Update()
    {
        if (_beforeHit) return;

        _plateTr.Rotate(_rotateSpeed * Time.deltaTime * Vector3.forward);
    }

    public int GetValue()
    {
        float maxValue = float.MinValue;
        int idx = -1;

        for (int i = 0; i < _numTr.Length; i++)
        {
            float value = Vector3.Dot(_numTr[i].up, _dartPoint.up);
            if (value > maxValue)
            {
                maxValue = value;
                idx = i;
            }
        }

        return idx + _minValue;
    }

    public override void Roll(CallbackMethod<int> callback)
    {
        _beforeHit = true;
        _dartPoint.SetParent(_plateTr);

        StartCoroutine(Stop(callback));
    }

    IEnumerator Stop(CallbackMethod<int> callback) {
        float spendTime = 0;

        while (spendTime < _stopTime) {
            yield return null;

            transform.Rotate(Mathf.Lerp(_rotateSpeed, 0, spendTime / _stopTime) * Time.deltaTime * Vector3.forward);
            spendTime += Time.deltaTime;

        }

        yield return new WaitForSeconds(1f);

        callback?.Invoke(GetValue());
    }


}
