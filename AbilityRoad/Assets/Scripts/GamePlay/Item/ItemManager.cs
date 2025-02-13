using EHTool;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager> {

    IDictionary<int, ItemData> _intToItemDict;
    IDictionary<ItemData, int> _itemToIntDict;

    protected override void OnCreate()
    {
        base.OnCreate();

        IDictionaryConnector<int, string>
            itemDictConnector = new JsonDictionaryConnector<int, string>();

        IDictionary<int, string> dict = itemDictConnector.ReadData("ItemInfor");
        _intToItemDict = new Dictionary<int, ItemData>();
        _itemToIntDict = new Dictionary<ItemData, int>();

        foreach (var data in dict) {
            _intToItemDict.Add(data.Key, AssetOpener.Import<ItemData>(data.Value));
        }

        foreach (var data in _intToItemDict) {
            _itemToIntDict.Add(data.Value, data.Key);
        }

    }

    public int RandomItemListByInt(int size = 3)
    {
        int randNum = 0;

        for (int i = 0; i < size; i++)
        {
            randNum *= _intToItemDict.Count;
            randNum += UnityEngine.Random.Range(0, _intToItemDict.Count);
        }

        return randNum;
    }

    public int ItemListToInt(IList<ItemData> itemList) {

        int retval = 0;

        for (int i = 0; i < itemList.Count; i++)
        {
            retval *= _intToItemDict.Count;
            retval += _itemToIntDict[itemList[i]];
        }

        return retval;
    }

    public IList<ItemData> ItemListFromInt(int value, int size = 3) {

        IList<ItemData> retval = new List<ItemData>();

        for (int i = 0; i < size; i++)
        {
            retval.Add(_intToItemDict[value % _intToItemDict.Count]);
            value /= _intToItemDict.Count;
        }

        return retval;
    }

}