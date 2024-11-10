using EHTool.UIKit;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GUIMatchSetting : GUIFullScreen
{
    [SerializeField] MatchInfor _infor;
    [SerializeField] string _path;
    [SerializeField] MatchCharacterDetail[] _details;

    [SerializeField] Transform _playerCntCursor;
    [SerializeField] Transform _mapCursor;
    [SerializeField] Transform _diceCursor;

    public void PlayerCntCursorPos(Transform target) {
        _playerCntCursor.position = target.position;
    }
    public void MapCursorPos(Transform target)
    {
        _mapCursor.position = target.position;

    }
    public void DiceCursorPos(Transform target)
    {
        _diceCursor.position = target.position;

    }

    public override void Open()
    {
        base.Open();
        SetDetails();
    }

    public void GenerateMatch()
    {
        for (int i = 0; i < _infor.PlayerInfors.Length; i++) {
            _infor.PlayerInfors[i].Name = _details[i].GetName();
        }

        UIManager.Instance.OpenGUI<GUIPlayground>("Playground").GenerateMatch(_infor);

    }

    public void SetPlayerCnt(int cnt) {

        MatchInfor.PlayerInfor[] def = _infor.PlayerInfors;

        _infor.PlayerInfors = new MatchInfor.PlayerInfor[cnt];

        for (int i = 0; i < cnt; i++)
        {
            if (i >= def.Length) {
                _infor.PlayerInfors[i] = new MatchInfor.PlayerInfor();
                continue;
            }
            _infor.PlayerInfors[i] = def[i];
            _infor.PlayerInfors[i].Name = _details[i].GetName();
        }

        SetDetails();
    }

    void SetDetails() {
        for (int i = 0; i < _details.Length; i++) {
            if (i >= _infor.PlayerInfors.Length)
            {
                _details[i].gameObject.SetActive(false);
                continue;
            }
            _details[i].gameObject.SetActive(true);
            _details[i].SetDefaultValue(i, _infor.PlayerInfors[i].CharacterCode, _infor.PlayerInfors[i].Name);
        }
    }

    public void SetDice(string diceCode) {
        _infor.MatchDice = diceCode;
    }
    public void SetPlayerName(int idx, string name) {
        if (idx >= _infor.PlayerInfors.Length) return;
        _infor.PlayerInfors[idx].Name = name;
    }

    public void SetPlayerCharacter(int idx, string name)
    {
        if (idx >= _infor.PlayerInfors.Length) return;
        _infor.PlayerInfors[idx].CharacterCode = name;
    }

    public void SetMap(string mapName) { 
        _infor.MapName = mapName;
    }
}
