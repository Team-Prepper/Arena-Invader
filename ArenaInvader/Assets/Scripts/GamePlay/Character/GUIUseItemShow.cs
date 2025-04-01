using EHTool.UIKit;
using UnityEngine;
using UnityEngine.UI;

public class GUIUseItemShow : GUIWindow {

    [SerializeField] private float _remaingTime = 1f;
    [SerializeField] private Text _message;

    public override void Open()
    {
        base.Open();
        Invoke(nameof(Close), _remaingTime);
    }

    public void SetUseItem(string itemCode) {
        _message.text = string.Format("{0} 사용", itemCode);
    }

}