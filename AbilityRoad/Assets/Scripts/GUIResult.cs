using EHTool.UIKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIResult : GUIFullScreen
{

    [SerializeField] Text _winnerName;
    [SerializeField] Image _winnerAttackImage;
    [SerializeField] Image _winnerTargetImage;

    public void SetWinner(BasePlayer winner) {
        _winnerName.text = winner.GetName();
        _winnerAttackImage.sprite = CharacterManager.Instance.GetPlayerAttackerSpr(winner.GetCharacterCode());
        _winnerTargetImage.sprite = CharacterManager.Instance.GetPlayerDamageSpr(winner.GetCharacterCode());
    }

    public override void Close()
    {
        SFXManager.Instance.PlayBGM("Start");
        base.Close();
    }
}
