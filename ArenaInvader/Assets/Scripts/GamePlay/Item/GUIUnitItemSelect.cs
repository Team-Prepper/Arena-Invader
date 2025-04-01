using UnityEngine;
using System.Collections.Generic;
using System;

public class GUIUnitItemSelect : MonoBehaviour
{
    [SerializeField] private GUIUnitItem _itemInfor;
    [SerializeField] private GameObject _light;
    [SerializeField] private GameObject _dark;

    private bool _enable;

    private Action<int> _onClickAction;
    private int _idx;

    public void SetSlot(IList<string> list, int idx, Action<int> action)
    {
        if (idx >= list.Count) {
            gameObject.SetActive(false);
            return;
        }
        
        gameObject.SetActive(true);
        _itemInfor.SetItemCode(list[idx]);
        _idx = idx;
        _onClickAction = action;
        _enable = true;
        _dark.SetActive(false);
        
        SetLight(false);
    }

    public void Select()
    {
        if (!_enable) return;
        _onClickAction?.Invoke(_idx);
    }

    public void SetLight(bool isActive) {
        _light.SetActive(isActive);

    }

    public void DisableSlot() {
        SetLight(false);
        _onClickAction = null;
        _dark.SetActive(true);
        _enable = false;
    }

}
