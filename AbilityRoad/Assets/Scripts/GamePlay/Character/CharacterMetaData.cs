using UnityEngine;

[CreateAssetMenu(fileName = "Data_Unit_", menuName = "ScriptableObjects/UnitData", order = 1)]
public class CharacterMetaData : ScriptableObject {
    
    public CharacterSprites CharacterSprs;
    public Character Prefab;
    public StatusElement[] Statuses;

}