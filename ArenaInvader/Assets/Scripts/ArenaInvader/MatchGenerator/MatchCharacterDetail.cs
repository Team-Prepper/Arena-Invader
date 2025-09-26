using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MatchCharacterDetail : MonoBehaviour
{
    [SerializeField] private GUIMatchCharacterSetting _mother;
    [SerializeField] private IGUIUnitPlayerInfor _playerInforUnit;
    [SerializeField] private InputField _nameSet;

    private int _idx;

    public void SetDefaultValue(IList<PlayerInfor> list, int idx, bool enableChange) {

        if (idx >= list.Count) {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        
        _idx = idx;
        _playerInforUnit.SetPlayerInfor(list[_idx].CharacterCode);
        _nameSet.text = list[_idx].Name;

        _nameSet.enabled = enableChange;

        if (!enableChange) return;

        _nameSet.onEndEdit.AddListener((value) =>
        {
            _mother.SetPlayerName(idx, value);
        });

    }

    public void SetCharacter(string code)
    {
        _playerInforUnit.SetPlayerInfor(code);
        _mother.SetPlayerCharacter(_idx, code);
    }

    internal string GetName()
    {
        return _nameSet.text;
    }

}