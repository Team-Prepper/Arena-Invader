using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GUIUnitInventory : MonoBehaviour
{
    [SerializeField] Image _icon;
    [SerializeField] private Button _selectButton;

    public void SetSlot(Sprite icon, UnityAction action)
    {
        _icon.sprite = icon;
        _selectButton.onClick.AddListener(action);
    }
    
    public void DisableSlot()
    {
        _selectButton.interactable = false;
    }
}
