using UnityEngine;
using UnityEngine.UI;

public class GUIUnitItem : MonoBehaviour
{
    [SerializeField] private Image _selectItemIcon;

    public void Disable() {
        gameObject.SetActive(false);
    }

    public virtual void SetItemCode(ItemData itemData) {
        gameObject.SetActive(true);
        _selectItemIcon.sprite = itemData.Icon;
    }

}
