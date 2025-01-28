using EHTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchGenerator : MonoBehaviour
{
    [SerializeField] MatchInfor _matchInfor;
    [SerializeField] GUICharacterActionSelector _guiActionSelector;
    [SerializeField] AICharacterActionSelector _aiActionSelector;

    [SerializeField] LocalCharacterController _localCharacterController;

    [SerializeField] Transform[] _playerPosition;

    public void SetMatchInfor(MatchInfor matchInfor) {
        _matchInfor = matchInfor;
    }

    public void Generate()
    {
        GameManager.Instance.Playground.Map = AssetOpener.ImportComponent<Map>(_matchInfor.MapName);

        for (int i = 0; i < _matchInfor.PlayerInfors.Length; i++) {
            ICharacterController cc = GameManager.Instance.Playground.InstantiateCC();

            cc.SetMatch(_matchInfor.PlayerInfors[i].CharacterCode.Equals("Player_AI") ? _aiActionSelector : _guiActionSelector);
            cc.SetTargetCharacter("Player", _matchInfor.PlayerInfors[i].Name, i);
            cc.Target.transform.position = _playerPosition[i].position;

        }

        SFXManager.Instance.PlayBGM("1st");
        GameManager.Instance.Playground.TurnStart();

    }
}
