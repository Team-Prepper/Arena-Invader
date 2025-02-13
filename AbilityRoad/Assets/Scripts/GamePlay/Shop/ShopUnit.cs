using UnityEngine;
using UnityEngine.UI;

public class ShopUnit : MonoBehaviour
{
    [SerializeField] Text _priceText;
    [SerializeField] Image _icon;
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
