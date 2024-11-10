using EHTool.UIKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIResult : GUIFullScreen
{

    [SerializeField] Text _winnerName;
    [SerializeField] Image _winnerIcon;

    public void SetWinner(BasePlayer winner) {
        _winnerName.text = winner.GetName();
        _winnerIcon.sprite = CharacterManager.Instance.GetPlayerSpr(winner.GetCharacterCode());
    }
}
