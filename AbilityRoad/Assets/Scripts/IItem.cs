using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IItem : MonoBehaviour
{
    [SerializeField] private int _price;

    public int Price
    {
        get => _price;
        set => _price = value;
    }
    [SerializeField] string _name;
    public string Name => _name; 
    [SerializeField] Sprite _icon;
    public Sprite Icon => _icon;
    
    [SerializeField] string _description;
    public string Description => _description;
    
    [SerializeField, Range(0,10)]
    private int _itemValue;
    public int ItemValue => _itemValue;
    
    public abstract void UseItem(BasePlayer player);
}
