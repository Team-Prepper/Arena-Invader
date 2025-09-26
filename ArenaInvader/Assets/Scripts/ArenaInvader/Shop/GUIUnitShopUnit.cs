using UnityEngine;
using UnityEngine.UI;

public class GUIUnitShopUnit : MonoBehaviour
{
    [SerializeField] private Text _priceText;
    [SerializeField] private Image _icon;
    [SerializeField] private Button _selectButton;

    public void SetSlot(ItemData item)
    {
        if (_priceText != null)
            _priceText.text = item.Price.ToString();
        _icon.sprite = item.Icon;
    }

    public void DisableSlot()
    {
        _icon.gameObject.SetActive(false);
        _selectButton.interactable = false;
    }
}
