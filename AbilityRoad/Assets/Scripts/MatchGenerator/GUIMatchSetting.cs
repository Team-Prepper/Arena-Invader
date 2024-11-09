using EHTool.UIKit;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Tilemaps;
using UnityEngine;

public class GUIMatchSetting : GUIFullScreen
{
    [SerializeField] MatchInfor _infor;
    [SerializeField] string _path;
    [SerializeField] MatchCharacterDetail[] _details;

    public override void Open()
    {
        base.Open();
        SetDetails();
    }

    public void GenerateMatch()
    {
        /*
        string json = JsonUtility.ToJson(_infor, true);
        File.WriteAllText(_path, json);*/

    }

    public void SetPlayerCnt(int cnt) {

        MatchInfor.PlayerInfor[] def = _infor.PlayerInfors;

        _infor.PlayerInfors = new MatchInfor.PlayerInfor[cnt];

        for (int i = 0; i < cnt; i++)
        {
            if (i >= def.Length) {
                continue;
            }
            _infor.PlayerInfors[i] = def[i];
        }

        SetDetails();
    }

    void SetDetails() {
        for (int i = 0; i < _details.Length; i++) {
            _details[i].gameObject.SetActive(i < _infor.PlayerInfors.Length);
        }
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
