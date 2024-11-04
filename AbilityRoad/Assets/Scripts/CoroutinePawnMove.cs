using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutinePawnMove : IPawnMove {
    public override void MoveTo(Vector3 goal, float moveTime, float stopTime, CallbackMethod callback)
    {
        StartCoroutine(_MoveTo(goal, moveTime, stopTime, callback));
    }

    IEnumerator _MoveTo(Vector3 goalPos, float moveTime, float stopTime, CallbackMethod callback)
    {
        float spendTime = 0;
        Vector3 originPos = transform.position;
        while (spendTime < moveTime)
        {
            yield return null;
            spendTime += Time.deltaTime;
            transform.position = Vector3.Lerp(originPos, goalPos, spendTime / moveTime);
        }

        yield return new WaitForSeconds(stopTime);

        transform.position = goalPos;
        callback?.Invoke();
    }
}