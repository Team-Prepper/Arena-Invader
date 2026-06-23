using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;

public class MatchCharacterDetail : MonoBehaviour
{
    [SerializeField] private GUIMatchCharacterSetting _mother;
    [SerializeField] private IGUIUnitPlayerInfor _playerInforUnit;
    [SerializeField] private InputField _nameSet;
    [SerializeField] private Button _humanButton;
    [SerializeField] private Button _aiButton;
    [SerializeField] private Graphic _humanButtonGraphic;
    [SerializeField] private Graphic _aiButtonGraphic;
    [SerializeField] private Color _selectedColor = Color.white;
    [SerializeField] private Color _unselectedColor = new Color(0.7f, 0.7f, 0.7f, 1f);

    private int _idx;
    private UnityAction<string> _nameChangedHandler;

    private void Awake()
    {
        _nameChangedHandler = OnNameChanged;
    }

    public void SetDefaultValue(IList<PlayerInfor> list, int idx, bool enableChange) {

        if (idx >= list.Count) {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        
        _idx = idx;
        _playerInforUnit.SetPlayerInfor(list[_idx].CharacterCode);
        _nameSet.onEndEdit.RemoveListener(_nameChangedHandler);
        _nameSet.text = list[_idx].Name;
        _nameSet.interactable = enableChange;
        SetAIButtonsInteractable(enableChange);
        RefreshAIState(list[_idx].IsAI);

        if (!enableChange) return;

        _nameSet.onEndEdit.AddListener(_nameChangedHandler);

    }

    private void OnNameChanged(string value)
    {
        _mother.SetPlayerName(_idx, value);
    }

    public void SetCharacter(string code)
    {
        _playerInforUnit.SetPlayerInfor(code);
        _mother.SetPlayerCharacter(_idx, code);
    }

    public void SetAI(bool isAI)
    {
        RefreshAIState(isAI);
        _mother.SetPlayerIsAI(_idx, isAI);
    }

    private void RefreshAIState(bool isAI)
    {
        if (_humanButtonGraphic != null)
        {
            _humanButtonGraphic.color = isAI ? _unselectedColor : _selectedColor;
        }

        if (_aiButtonGraphic != null)
        {
            _aiButtonGraphic.color = isAI ? _selectedColor : _unselectedColor;
        }
    }

    private void SetAIButtonsInteractable(bool interactable)
    {
        if (_humanButton != null)
        {
            _humanButton.interactable = interactable;
        }

        if (_aiButton != null)
        {
            _aiButton.interactable = interactable;
        }
    }

    internal string GetName()
    {
        return _nameSet.text;
    }

    private void OnDestroy()
    {
        if (_nameChangedHandler != null)
        {
            _nameSet.onEndEdit.RemoveListener(_nameChangedHandler);
        }
    }

}
