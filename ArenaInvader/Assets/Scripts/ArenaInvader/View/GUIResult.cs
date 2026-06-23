using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyH.Unity.UI;
using EasyH.Unity.SoundKit;

public class GUIResult : GUIFullScreen
{

    [SerializeField] Text _winnerName;
    [SerializeField] Image _winnerAttackImage;
    [SerializeField] Image _winnerTargetImage;

    public void SetWinner(IStatus winner) {
        if (winner == null)
        {
            return;
        }

        CharacterSprites sprites =
            CharacterManager.Instance.GetCharacterSprites(winner.CharacterCode);

        _winnerName.text = winner.Name;
        _winnerAttackImage.sprite = sprites.CharacterAttack;
        _winnerTargetImage.sprite = sprites.CharacterDamage;
    }

    public override void Close()
    {
        SoundManager.Instance.PlayBGM("Start");
        base.Close();
    }
}
