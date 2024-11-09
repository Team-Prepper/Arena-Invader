using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopUnit : MonoBehaviour
{
    [SerializeField] Text _priceText;
    [SerializeField] Image _icon;
    [SerializeField] private Button _selectButton;

    public void SetSlot(int price, Sprite icon, UnityAction action)
    {
        _priceText.text = price.ToString();
        _icon.sprite = icon;
        _selectButton.onClick.AddListener(action);
    }

    public void DisableSlot()
    {
        _selectButton.interactable = false;
    }
}
