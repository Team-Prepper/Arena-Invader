using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IItem : MonoBehaviour
{
    public int Price { get; }
    public string Name { get; }
    public Sprite Icon { get; }
    
    public string Description { get; }
    
    public abstract void UseItem();
}
