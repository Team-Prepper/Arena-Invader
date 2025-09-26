using UnityEngine;

[CreateAssetMenu(fileName = "Data_Item_", menuName = "ScriptableObjects/ItemData", order = 1)]
public class ItemData : ScriptableObject {

    public IItem Item;
    public Sprite Icon;

    public string Code;
    public string Name;
    public string Desc;
    public int Price;

    [SerializeField, Range(0, 10)]
    public int ItemValue;

}
