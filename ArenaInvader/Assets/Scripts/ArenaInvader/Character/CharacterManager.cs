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

        IDictionary<string, string> source = connector.ReadData("CharacterInfor");
        if (source == null)
        {
            Debug.LogWarning("CharacterInfor data could not be loaded.");
            return;
        }

        foreach (var value in source)
        {
            CharacterMetaData data = ResourceManager.Instance.ResourceConnector.
                Import<CharacterMetaData>(value.Value);

            if (data == null)
            {
                Debug.LogWarning(
                    $"Character data '{value.Key}' could not be loaded from '{value.Value}'.");
                continue;
            }

            _dic[value.Key] = data;
        }
    }

    private CharacterMetaData GetCharacterData(string code)
    {
        if (string.IsNullOrWhiteSpace(code) || !_dic.TryGetValue(code, out CharacterMetaData data))
        {
            _dic.TryGetValue("Player", out data);
        }

        if (data == null)
        {
            throw new KeyNotFoundException(
                $"Character data not found for code '{code}' and fallback 'Player'.");
        }

        return data;
    }

    public CharacterSprites GetCharacterSprites(string code)
    {
        return GetCharacterData(code).CharacterSprs;
    }

    public int GetCharacterDefaultHP(string code)
    {
        return GetCharacterData(code).DefaultHP;
    }

    public StatusElement[] GetStatuses(string code)
    {
        return GetCharacterData(code).Statuses;

    }

    public GameObject SpawnPlayer(string code)
    {
        CharacterMetaData data = GetCharacterData(code);
        if (data.Prefab == null)
        {
            throw new MissingReferenceException(
                $"Character prefab is missing for code '{code}'.");
        }

        return GameObject.Instantiate(data.Prefab);
    }
    
}
