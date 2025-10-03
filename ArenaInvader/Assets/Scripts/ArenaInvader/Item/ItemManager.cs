using System.Collections.Generic;
using EasyH;
using EasyH.Unity;

public class ItemManager : Singleton<ItemManager> {

    private IDictionary<string, ItemData> _itemDict;
    private IDictionary<int, string> _intToItemDict;
    private IDictionary<string, int> _itemToIntDict;

    protected override void OnCreate()
    {
        base.OnCreate();

        IDictionaryConnector<int, string> itemDictConnector
            = new JsonDictionaryConnector<int, string>();

        IDictionary<int, string> dict = itemDictConnector.ReadData("ItemInfor");

        _itemDict = new Dictionary<string, ItemData>();

        _intToItemDict = new Dictionary<int, string>();
        _itemToIntDict = new Dictionary<string, int>();

        foreach (var data in dict) {
            ItemData d = ResourceManager.Instance.
                ResourceConnector.Import<ItemData>(data.Value);
            _itemDict.Add(d.Code, d);
            _intToItemDict.Add(data.Key, d.Code);
        }

        foreach (var data in _intToItemDict) {
            _itemToIntDict.Add(data.Value, data.Key);
        }

    }

    public ItemData GetItemData(string itemCode) {
        return _itemDict[itemCode];
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

    public int ItemListToInt(IList<string> itemList) {

        int retval = 0;

        for (int i = 0; i < itemList.Count; i++)
        {
            retval *= _intToItemDict.Count;
            retval += _itemToIntDict[itemList[i]];
        }

        return retval;
    }

    public IList<string> ItemListFromInt(int value, int size = 3) {

        IList<string> retval = new List<string>();

        for (int i = 0; i < size; i++)
        {
            retval.Add(_intToItemDict[value % _intToItemDict.Count]);
            value /= _intToItemDict.Count;
        }

        return retval;
    }

}