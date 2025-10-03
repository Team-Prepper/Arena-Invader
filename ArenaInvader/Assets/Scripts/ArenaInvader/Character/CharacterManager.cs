using System.Collections.Generic;
using UnityEngine;
using EasyH;
using EasyH.Unity;

public class CharacterManager : Singleton<CharacterManager> {

    private IDictionary<string, CharacterMetaData> _dic;

    protected override void OnCreate()
    {
        _dic = new Dictionary<string, CharacterMetaData>();

        IDictionaryConnector<string, string> connector
            = new JsonDictionaryConnector<string, string>();

        foreach (var value in connector.ReadData("CharacterInfor"))
        {
            _dic.Add(value.Key,
                ResourceManager.Instance.ResourceConnector.
                    Import<CharacterMetaData>(value.Value));
        }
    }

    public CharacterSprites GetCharacterSprites(string code)
    {
        if (!_dic.ContainsKey(code)) code = "Player";
        return _dic[code].CharacterSprs;
    }

    public int GetCharacterDefaultHP(string code)
    { 
        if (!_dic.ContainsKey(code)) code = "Player";
        return _dic[code].DefaultHP;
        
    }

    public StatusElement[] GetStatuses(string code)
    {
        if (!_dic.ContainsKey(code)) code = "Player";
        return _dic[code].Statuses;

    }

    public GameObject SpawnPlayer(string code) {
        return GameObject.Instantiate(_dic[code].Prefab);
    }
    
}