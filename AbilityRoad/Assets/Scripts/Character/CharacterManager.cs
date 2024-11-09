using EHTool;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class CharacterManager : Singleton<CharacterManager> {
    class CharacterData {
        internal string name;
        internal string path;

        internal void Read(XmlNode node)
        {
            name = node.Attributes["name"].Value;
            path = node.Attributes["path"].Value;
        }
    }

    IDictionary<string, CharacterMetaData> _dic;

    protected override void OnCreate()
    {
        _dic = new Dictionary<string, CharacterMetaData>();
        XmlDocument xmlDoc = AssetOpener.ReadXML("CharacterInfor");

        XmlNodeList nodes = xmlDoc.SelectNodes("List/Element");

        for (int i = 0; i < nodes.Count; i++)
        {
            CharacterData charData = new CharacterData();
            charData.Read(nodes[i]);

            _dic.Add(charData.name, AssetOpener.Import<CharacterMetaData>(charData.path));
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