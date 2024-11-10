using EHTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchGenerator : MonoBehaviour
{
    [SerializeField] MatchInfor _matchInfor;


    [SerializeField] Transform[] _playerPosition;

    public void SetMatchInfor(MatchInfor matchInfor) {
        _matchInfor = matchInfor;
    }

    public void Generate()
    {
        GameManager.Instance.Playground.Map = AssetOpener.ImportComponent<Map>(_matchInfor.MapName);

        for (int i = 0; i < _matchInfor.PlayerInfors.Length; i++) {
            BasePlayer player =
                CharacterManager.Instance.SpawnPlayer(_matchInfor.PlayerInfors[i].CharacterCode);
            player.SetInitial(i, _matchInfor.PlayerInfors[i].Name);
            player.transform.position = _playerPosition[i].position;
        }
        SFXManager.Instance.PlayBGM("1st");
        GameManager.Instance.Playground.TurnStart();
    }
}
