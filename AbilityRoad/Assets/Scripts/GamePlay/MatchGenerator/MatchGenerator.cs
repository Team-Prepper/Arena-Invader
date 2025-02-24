using EHTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchGenerator : MonoBehaviour
{
    [SerializeField] LocalMatchInfor _defaultMatchiInfor;
    IMatchInfor _matchInfor;

    [SerializeField] LocalCharacterController _localCharacterController;

    [SerializeField] Transform[] _playerPosition;

    public void SetMatchInfor(IMatchInfor matchInfor) {
        _matchInfor = matchInfor;
    }

    public void Generate()
    {
        if (_matchInfor == null) {
            _matchInfor = _defaultMatchiInfor;
        }

        for (int i = 0; i < _matchInfor.PlayerInfors.Count; i++) {
            ICharacterController cc = GameManager.Instance.Playground.InstantiateCC(_playerPosition[i].position);

            cc.SetTargetCharacter(
                _matchInfor.PlayerInfors[i].Name,
                _matchInfor.PlayerInfors[i].CharacterCode, i);

        }

        SFXManager.Instance.PlayBGM("1st");
        GameManager.Instance.Playground.PlayReady();

    }

}
