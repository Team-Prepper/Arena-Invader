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

            cc.SetTargetCharacter("Player", _matchInfor.PlayerInfors[i].Name,
                _matchInfor.PlayerInfors[i].CharacterCode.Equals("Player_AI") ? _aiActionSelector : _guiActionSelector);
            cc.Target.SetInitial(cc, i, _matchInfor.PlayerInfors[i].Name);
            cc.Target.transform.position = _playerPosition[i].position;

        }

        SFXManager.Instance.PlayBGM("1st");
        GameManager.Instance.Playground.TurnStart();

    }
}
