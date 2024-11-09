using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDice : IDice {

    static int random = 1;

    [SerializeField] Vector2Int _range;
    [SerializeField] Text _num;

    public override void Initial() {
        _num.text = string.Format("{0}", random);
    }

    public override void Roll(CallbackMethod<int> callback) {
        StartCoroutine(Dice(callback));
    }

    IEnumerator Dice(CallbackMethod<int> callback)
    {

        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(.2f);
            _num.text = string.Format("{0}", Random.Range(_range.x, _range.y));
        }
        random = Random.Range(_range.x, _range.y);

        _num.text = string.Format("{0}", random);

        yield return new WaitForSeconds(.2f);

        callback?.Invoke(random);
    }

}
