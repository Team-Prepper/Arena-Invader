using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MatchCharacterDetail : MonoBehaviour
{
    [SerializeField] GUIMatchSetting _mother;
    [SerializeField] Image _icon;
    [SerializeField] InputField _nameSet;

    int _idx;

    public void SetDefaultValue(int idx, string character, string name) {
        _idx = idx;
        _icon.sprite = CharacterManager.Instance.GetPlayerSpr(character);
        _nameSet.text = name;

        _nameSet.onSubmit.AddListener((value) =>
        {
            _mother.SetPlayerName(idx, value);
        });
    }

    public void SetCharacter(string code)
    {
        _icon.sprite = CharacterManager.Instance.GetPlayerSpr(code);
        _mother.SetPlayerCharacter(_idx, code);
    }

    internal string GetName()
    {
        return _nameSet.text;
    }
}
