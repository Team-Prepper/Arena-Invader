using UnityEngine;

[CreateAssetMenu(fileName = "Data_Item_", menuName = "ScriptableObjects/ItemData", order = 1)]
public class ItemData : ScriptableObject {

    public IItem Item;

    public int Price;
    public string Name;
    public Sprite Icon;
    public string Desc;

    [SerializeField, Range(0, 10)]
    public int ItemValue;

}
