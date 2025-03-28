using UnityEngine;
using UnityEngine.UI;
using System;

public class GUIUnitItemSelect : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private GameObject _light;
    [SerializeField] private GameObject _dark;

    private bool _enable;

    private Action<int> _onClickAction;
    private int _idx;

    public void SetSlot(Sprite icon, int idx, Action<int> action)
    {
        _icon.sprite = icon;
        _idx = idx;
        _onClickAction = action;
        _enable = true;
        _dark.SetActive(false);
        
        DisSelect();
    }

    public void Select()
    {
        if (!_enable) return;
        _onClickAction?.Invoke(_idx);
        _light.SetActive(true);
    }

    public void DisSelect()
    {
        _light.SetActive(false);
    }

    public void DisableSlot() {
        DisSelect();
        _onClickAction = null;
        _dark.SetActive(true);
        _enable = false;
    }

}
