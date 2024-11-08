using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchGenerator : MonoBehaviour
{
    [SerializeField] Character[] _playerPrefab;
    [SerializeField] Map _mapPrefab;
    [SerializeField] Transform[] _playerPosition;

    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }


    void Generate()
    {
        GameManager.Instance.Playground.Map = Instantiate(_mapPrefab);
        for (int i = 0; i < _playerPosition.Length; i++) {
            Character player = Instantiate(_playerPrefab[i]);
            player.SetInitial(i);
            player.transform.position = _playerPosition[i].position;
        }
        GameManager.Instance.Playground.TurnStart();
    }
}
