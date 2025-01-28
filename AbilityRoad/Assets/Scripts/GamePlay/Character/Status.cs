using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour {

    [System.Serializable]
    public class StatusElement {
        [SerializeField] internal int _attack;
        [SerializeField] internal int _defence;

        StatusElement()
        {
            _attack = 0;
            _defence = 0;
        }
    }

    [SerializeField] private StatusElement _addedValue;
    [SerializeField] private StatusElement[] _levelStatus;

    public int GetAttackValue(int level)
    {
        return _levelStatus[Mathf.Min(_levelStatus.Length - 1, level)]._attack + _addedValue._attack;
    }
    public int GetDefenseValue(int level)
    {
        return _levelStatus[Mathf.Min(_levelStatus.Length - 1, level)]._defence + _addedValue._defence;
    }

    public void AddAttackValue(int amount)
    {
        _addedValue._attack += amount;
    }

    public void AddDefenceValue(int amount)
    {
        _addedValue._defence += amount;
    }

}
