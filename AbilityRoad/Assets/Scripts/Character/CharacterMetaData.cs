using UnityEngine;

[CreateAssetMenu(fileName = "Data_Unit_", menuName = "ScriptableObjects/UnitData", order = 1)]
public class CharacterMetaData : ScriptableObject {
    
    public Sprite CharacterIcon;
    public BasePlayer Prefab;

}