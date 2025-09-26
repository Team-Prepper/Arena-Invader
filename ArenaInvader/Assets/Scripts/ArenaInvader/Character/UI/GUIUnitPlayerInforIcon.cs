using UnityEngine.UI;
using UnityEngine;

public class GUIUnitPlayerInforIcon: IGUIUnitPlayerInfor {

    [SerializeField] private Image _icon;

    public override void SetPlayerInfor(string code) {
        _icon.sprite = CharacterManager.Instance.
            GetCharacterSprites(code).CharacterIcon;

    }

}