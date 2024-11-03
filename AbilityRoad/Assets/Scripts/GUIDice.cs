using EHTool.UIKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIDice : GUIPopUp
{
    static int random = 1;

    [SerializeField] Vector2Int _range;

    [SerializeField] Text _num;

    CallbackMethod<int> _callback;

    bool _isRolling;

    public override void Open()
    {
        base.Open();
        _num.text = string.Format("{0}", random);
        _isRolling = false;
    }

    public void SetCallback(CallbackMethod<int> callback)
    {
        _callback = callback;
    }

    public void Roll() {
        if (_isRolling) return;

        StartCoroutine(Dice());
        _isRolling = true;
    }

    IEnumerator Dice() {

        for (int i = 0; i < 3; i++) {
            yield return new WaitForSeconds(.2f);
            _num.text = string.Format("{0}", Random.Range(_range.x, _range.y));
        }
        random = Random.Range(_range.x, _range.y);

        _num.text = string.Format("{0}", random);

        yield return new WaitForSeconds(.2f);

        _callback?.Invoke(random);
        Close();
    }

}
