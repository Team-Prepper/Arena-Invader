using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using EasyH.Unity.SoundKit;

public class CustomButton : Button
{
    [SerializeField] private string _soundKey = "ButtonSelect";

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        SoundManager.Instance.PlaySFX(_soundKey);
    }

}
