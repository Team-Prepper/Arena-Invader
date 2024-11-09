using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GUIBaronInfo : MonoBehaviour
{
    [SerializeField] Image _baronImage;
    [SerializeField] Text _baronName;
    [SerializeField] Text _baronHP;
    
    public void SetBaronInfo(Character character)
    {
        _baronImage.sprite = CharacterManager.Instance.GetPlayerSpr(character.GetCharacterCode());
        _baronName.text = "Baron";
        _baronHP.text = character.GetHealth().ToString();
    }
}
