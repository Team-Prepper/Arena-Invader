using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteDice : IDice {

    [SerializeField] Vector2 _speedRange = Vector2.right;
    [SerializeField] float _maxTime = 1f;
    [SerializeField] Transform _rouletteTr;

    [SerializeField] Transform[] _numTr;
    [SerializeField] int _minValue;

    public override void Initial() {
        _rouletteTr.eulerAngles = Vector3.forward * Random.Range(-180, 180);
    }

    public override void Roll(CallbackMethod<int> callback)
    {
        StartCoroutine(Dice(callback));
    }

    int GetValue() {
        float maxValue = float.MinValue;
        int idx = -1;

        for (int i = 0; i < _numTr.Length; i++) {
            float value = Vector3.Dot(_numTr[i].up, Vector3.up);
            if (value > maxValue) {
                maxValue = value;
                idx = i;
            }
        }

        return idx + _minValue;
    }

    IEnumerator Dice(CallbackMethod<int> callback)
    {
        float spendTime = 0;
        float speed = Random.Range(_speedRange.x, _speedRange.y);
        while (spendTime < _maxTime) {
            _rouletteTr.Rotate(Vector3.forward * Mathf.Lerp(speed, 0, spendTime / _maxTime) * Time.deltaTime);
            spendTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(.2f);

        callback?.Invoke(GetValue());
    }
}
