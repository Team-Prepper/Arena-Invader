using UnityEngine;

[CreateAssetMenu(fileName = "Data_Character_", menuName = "ScriptableObjects/CharacterData", order = 1)]
public class CharacterMetaData : ScriptableObject {
    
    public CharacterSprites CharacterSprs;
    public GameObject Prefab;
    public StatusElement[] Statuses;

}