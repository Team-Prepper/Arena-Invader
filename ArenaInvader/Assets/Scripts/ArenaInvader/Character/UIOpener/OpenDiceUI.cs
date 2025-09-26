using System;
using UnityEngine;
using EasyH.Unity.UI;

public class OpenDiceUI : MonoBehaviour, IOpenDice
{
    public void Initial(IPlayableCharacter cc)
    {

    }

    public GUIDice OpenDice(Action<int> callback)
    {

        GUIDice gui = UIManager.Instance.OpenGUI<GUIDice>
            (GameManager.Instance.MatchInfor.MatchDice);

        int seed = UnityEngine.Random.Range(0, 100);
        gui.SetSeed(DateTime.Now.Millisecond);

        gui.SetCallback(callback);

        return gui;
        
    }
}