using System.Collections.Generic;
using UnityEngine;
using EasyH.Gaming.PathBased;

public class GameMap : PathMap {

    [System.Serializable]
    public class RaidInfo {
        public int StartTurn;
        public string SpawnCode;
    }

    [SerializeField] RaidInfo[] _raidInfors;

    private IStatus _object;

    private IDictionary<int, string> _raidDict;
    
    private int currentTurn = 0;

    private void Start()
    {
        PathManager.Instance.Map = this;
        BoardManager.Instance.Map = this;
        
        _raidDict = new Dictionary<int, string>();

        if (_raidInfors == null)
        {
            return;
        }

        for (int i = 0; i < _raidInfors.Length; i++)
        {
            if (_raidInfors[i] == null) continue;
            _raidDict[_raidInfors[i].StartTurn] = _raidInfors[i].SpawnCode;
        }

    }

    public IStatus GetObject() {
        if (_object == null) return null;
        if (_object.IsAlive()) return _object;

        _object = null;
        return _object;
    }

    public void StartNewTurn(int turn) {
        currentTurn = turn;

        if (_raidDict == null || !_raidDict.ContainsKey(turn)) return;
        
        GameManager.Instance.Playground.InstantiateStatus();

        //SFXManager.Instance.PlayBGM("2nd");
        
    }

    public int GetLeftBaronTurn()
    {
        if (_raidDict == null || _raidDict.Count == 0)
        {
            return -1;
        }

        List<int> keys = new List<int>(_raidDict.Keys);
        keys.Sort();
        return keys[0] - currentTurn;
    }

}
