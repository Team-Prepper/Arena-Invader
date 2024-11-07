using System.Collections;
using UnityEngine;

public class TeleportMoveTo : IMoveTo {

    [SerializeField] float _stall = 0.1f;
    [SerializeField] float _delay = 0.1f;

	CallbackMethod _callback;

	public override void MoveTo(Vector3 pos, CallbackMethod callback)
    {
        _callback = callback;
        StartCoroutine(_MoveTo(pos));
    }

    IEnumerator _MoveTo(Vector3 pos) {
        yield return new WaitForSeconds(_stall);
        gameObject.transform.localScale = Vector3.zero;
        yield return new WaitForSeconds(_delay);
        gameObject.transform.position = pos;
        gameObject.transform.localScale = Vector3.one;
		yield return new WaitForSeconds(_stall);
        _callback?.Invoke();
	}
}