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
        _winnerName.text = winner.Name;
        _winnerAttackImage.sprite = CharacterManager.Instance.GetCharacterSprites(winner.CharacterCode).CharacterAttack;
        _winnerTargetImage.sprite = CharacterManager.Instance.GetCharacterSprites(winner.CharacterCode).CharacterDamage;
    }

    public override void Close()
    {
        SoundManager.Instance.PlayBGM("Start");
        base.Close();
    }
}
