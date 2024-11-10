using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class D6Dice : IDice {

    private Dictionary<Vector3Int, int> _diceInfor = new Dictionary<Vector3Int, int>
    {
        { new Vector3Int( 0,  0,  1), 5 },
        { new Vector3Int( 0,  1,  0), 4 },
        { new Vector3Int(-1,  0,  0), 1 },
        { new Vector3Int( 1,  0,  0), 6 },
        { new Vector3Int( 0, -1,  0), 3 },
        { new Vector3Int( 0,  0, -1), 2 },
    };

    public int Value { get; private set; }
    private bool _isRolling;

    private float torqueMin = 5f;
    private float torqueMax = 10f;
    private float throwStrength = 10f;
    private float _waitTime = 1f;

    [SerializeField] private UnityEvent _diceStopEvent;

    private Rigidbody rb;
    private Vector3 originPos = new Vector3(0, 3, 0);

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void Initial()
    {
        Value = 0;
        _isRolling = false;
        rb.useGravity = false;
        rb.isKinematic = false;
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
        transform.localPosition = originPos;
    }
    
    [ContextMenu("Execute Function")]
    private void MyFunction()
    {
        RollDice();
    }
    
    public override void Roll(CallbackMethod<int> callback)
    {
        _diceStopEvent.AddListener(() => callback(Value));
        RollDice();
    }
    public void RollDice()
    {
        if (_isRolling) return;

        _isRolling = true;
        transform.localPosition = originPos;
        rb.useGravity = true;
        rb.velocity = Vector3.up * throwStrength;
        rb.AddForce(Random.insideUnitSphere * Random.Range(5, 10), ForceMode.Impulse);
        rb.AddTorque(Random.insideUnitSphere * Random.Range(torqueMin, torqueMax), ForceMode.Impulse);

        StartCoroutine(CheckIdle());
    }

    private IEnumerator CheckIdle()
    {
        Vector3 lastPosition = rb.position;
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (Vector3.Distance(rb.position, lastPosition) < 0.01f)
            {
                break;
            }
            lastPosition = rb.position;
        }

        CheckRollResult();
    }

    private void CheckRollResult()
    {
        Vector3Int roundedOrientation = new Vector3Int(
            Mathf.RoundToInt(Vector3.Dot(transform.right, Vector3.up)),
            Mathf.RoundToInt(Vector3.Dot(transform.up, Vector3.up)),
            Mathf.RoundToInt(Vector3.Dot(transform.forward, Vector3.up))
        );

        if (_diceInfor.TryGetValue(roundedOrientation, out int rollValue))
        {
            Value = rollValue;
            Debug.Log("Dice Value : " + Value);
            StartCoroutine(StopRolling());
        }
        else
        {
            RollDice(); // Roll again if orientation is invalid
        }
    }

    private IEnumerator StopRolling()
    {
        yield return new WaitForSeconds(_waitTime);
        _diceStopEvent.Invoke();
        _isRolling = false;
    }
}