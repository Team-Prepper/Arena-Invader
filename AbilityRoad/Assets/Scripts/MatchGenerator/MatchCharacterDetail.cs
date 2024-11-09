using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchCharacterDetail : MonoBehaviour
{
    [SerializeField] GUIMatchSetting _mother;
    [SerializeField] InputField _nameSet;

    public void SetDefaultValue(string name) {
        _nameSet.text = name;
    }
}
