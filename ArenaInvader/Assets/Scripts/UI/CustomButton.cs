using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : Button
{
    [SerializeField] private string _soundKey = "ButtonSelect";

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        SFXManager.Instance.PlaySFX(_soundKey);
    }

}
