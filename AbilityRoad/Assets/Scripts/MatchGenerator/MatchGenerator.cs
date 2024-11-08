using EHTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchGenerator : MonoBehaviour
{
    [SerializeField] MatchInfor _matchInfor;


    [SerializeField] Transform[] _playerPosition;

    // Start is called before the first frame update
    void Awake()
    {
        Generate();
    }


    void Generate()
    {
        GameManager.Instance.Playground.Map = AssetOpener.Import<Map>(_matchInfor.MapName);

        for (int i = 0; i < _matchInfor.PlayerInfors.Length; i++) {
            BasePlayer player = AssetOpener.Import<BasePlayer>(_matchInfor.PlayerInfors[i].CharacterName);
            player.SetInitial(i, _matchInfor.PlayerInfors[i].Name);
            player.transform.position = _playerPosition[i].position;
        }
        GameManager.Instance.Playground.TurnStart();
    }
}
