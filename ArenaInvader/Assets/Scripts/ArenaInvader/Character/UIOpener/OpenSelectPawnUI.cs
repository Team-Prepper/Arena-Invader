using System;
using UnityEngine;
using EasyH.Unity.UI;

public class OpenSelectPawnUI : MonoBehaviour, IOpenSelectPawn
{
    private IPlayableCharacter _cc;
    
    public void Initial(IPlayableCharacter cc)
    {
        _cc = cc;
    }

    public GUISelectMovePawn OpenSelectMovePawn(int value, int addedValue)
    {
        GUISelectMovePawn movePawn =
            UIManager.Instance.OpenGUI<GUISelectMovePawn>("SelectMovePawn");

        _cc.PawnOwner.OnPawnChoose();

        movePawn.SetPlayer(_cc, value, addedValue);

        return movePawn;
    }
}