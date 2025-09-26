using System;
using UnityEngine;
using EasyH.Unity.UI;

public class OpenBattleUI : MonoBehaviour, IOpenBattle
{
    public void Initial(IPlayableCharacter cc)
    {
        
    }

    public GUIBattle OpenBattle(int attacker, int target, Action callback)
    {
        GUIBattle battle = UIManager.Instance.OpenGUI<GUIBattle>("Battle");

        battle.BattleSet(attacker, target);
        battle.StartBattle(callback);

        return battle;
    }
}