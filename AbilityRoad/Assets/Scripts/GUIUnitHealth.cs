using UnityEngine;
using UnityEngine.UI;

public class GUIUnitHealth : IGUIUnitHealth {

    [SerializeField] Text _healthTxt;
    [SerializeField] string _format = "{0}";

    public override void SetHealth(int amount) {
        _healthTxt.text = string.Format(_format, amount);
    }
}
