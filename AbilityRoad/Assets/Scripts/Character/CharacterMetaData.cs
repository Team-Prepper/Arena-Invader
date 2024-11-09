using UnityEngine;

[CreateAssetMenu(fileName = "Data_Unit_", menuName = "ScriptableObjects/UnitData", order = 1)]
public class CharacterMetaData : ScriptableObject {
    
    public Sprite CharacterIcon;
    public Sprite CharacterAttack;
    public Sprite CharacterDamage;
    public Sprite CharacterStand;
    public Character Prefab;

}