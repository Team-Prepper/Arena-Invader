using EHTool;
using EHTool.UIKit;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    IDictionary<string, CharacterData> _dic;

    protected override void OnCreate()
    {
        XmlDocument xmlDoc = AssetOpener.ReadXML("CharacterInfor");

        XmlNodeList nodes = xmlDoc.SelectNodes("List/Element");

        for (int i = 0; i < nodes.Count; i++)
        {
            CharacterData charData = new CharacterData();
            charData.Read(nodes[i]);

            _dic.Add(charData.name, charData);
        }
    }

    public BasePlayer SpawnPlayer(string code) {
        return AssetOpener.Import<BasePlayer>(_dic[code].path);
    }
    

}
