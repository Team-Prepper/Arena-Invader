using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutinePawnMove : IPawnMove {

    [SerializeField] float _startStall;
    [SerializeField] float _moveStall;
    [SerializeField] float _moveTime = 0.5f;
    
    [SerializeField] ParticleSystem _moveEffect;

    CallbackMethod _arriveCallback;
    CallbackMethod<IPlate> _moveEndCallback;

    IPlate _beforePlate;
    IPlate _nowPlate;
    int _movePoint;

    public override void MoveTo(IPlate startPos, Pawn target, int amount, CallbackMethod arrive, CallbackMethod<IPlate> moveEnd)
    {
        _movePoint = amount;

        _arriveCallback = arrive;
        _moveEndCallback = moveEnd;

        startPos.Leave(target, MoveTo);
    }

    public override IPlate Predict(IPlate startPos, Pawn target, int amount)
    {
        IPlate retval = startPos;
        IPlate beforePlate = null;

        for (int i = 0; i < amount; i++)
        {

            if (retval == null) return null;
            
            retval.NextPlate(beforePlate, (plate) => {
                beforePlate = retval;
                retval = plate;
            });
        }
        return retval;
    }

    public override void DisposeTo(Vector3 pos)
    {
        StartCoroutine(_MoveTo(pos, _moveTime, 0, null));
    }

    private void MoveTo(IPlate plate)
    {

        if (plate == null)
        {
            _arriveCallback?.Invoke();
            return;
        }

        _movePoint--;

        StartCoroutine(_MoveTo(plate.transform.position, _moveTime, _moveStall, () => {
            _beforePlate = _nowPlate;
            _nowPlate = plate;

            if (_movePoint < 1)
            {
                _moveEndCallback?.Invoke(plate);
                return;
            }

            plate.NextPlate(_beforePlate, MoveTo);

        }));
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
        
        _moveEffect.Play();
        SFXManager.Instance.PlaySFX("FootStep");
        yield return new WaitForSeconds(stopTime);
        
        transform.position = goalPos;
        callback?.Invoke();
    }
}