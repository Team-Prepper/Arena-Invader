using EHTool;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Map : MonoBehaviour {

    [System.Serializable]
    public class RaidInfor {
        public int StartTurn;
        public string SpawnCode;
    }

    [SerializeField] IPlate _startPlate;
    [SerializeField] RaidInfor[] _raidInfors;

    ObjectCharacter _object;

    IDictionary<int, string> _raidDict;
    
    private int currentTurn = 0;

    private void Start()
    {
        _raidDict = new Dictionary<int, string>();

        for (int i = 0; i < _raidInfors.Length; i++) {
            _raidDict.Add(_raidInfors[i].StartTurn, _raidInfors[i].SpawnCode);
        }
    }

    public IPlate GetStartPlate() {
        return _startPlate;
    }

    public ObjectCharacter GetObject() {
        if (_object == null) return null;
        if (_object.IsAlive()) return _object;

        _object = null;
        return _object;
    }

    public void StartNewTurn(int turn) {
        currentTurn = turn;
        if (!_raidDict.ContainsKey(turn)) return;

        _object = AssetOpener.ImportComponent<ObjectCharacter>(_raidDict[turn]);
        _object.transform.SetParent(transform);
        SFXManager.Instance.PlayBGM("2nd");
    }

    public int GetLeftBaronTurn()
    {
        List<int> keys = new List<int>(_raidDict.Keys);
        keys.Sort();
        return keys[0] - currentTurn;
    }

}