using EHTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : Singleton<CharacterManager> {

    IDictionary<string, CharacterMetaData> _dic;

    protected override void OnCreate()
    {
        _dic = new Dictionary<string, CharacterMetaData>();

        IDictionaryConnector<string, string> connector
            = new JsonDictionaryConnector<string, string>();

        foreach (var value in connector.ReadData("CharacterInfor"))
        {
            _dic.Add(value.Key, AssetOpener.Import<CharacterMetaData>(value.Value));
        }
    }

    public Sprite GetPlayerAttackerSpr(string code) {
        return _dic[code].CharacterAttack;
    }

    public Sprite GetPlayerDamageSpr(string code)
    {
        return _dic[code].CharacterDamage;
    }

    public Sprite GetPlayerStandSpr(string code)
    {
        return _dic[code].CharacterStand;
    }

    public Sprite GetPlayerSpr(string code) {
        return _dic[code].CharacterIcon;
    }

    public BasePlayer SpawnPlayer(string code) {
        return (BasePlayer)Object.Instantiate(_dic[code].Prefab);
    }
    
}