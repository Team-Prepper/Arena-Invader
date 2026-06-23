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
        if (GameManager.Instance?.MatchInfo == null)
        {
            throw new MissingReferenceException(
                $"{name} cannot open dice UI without match information.");
        }

        GUIDice gui = UIManager.Instance.OpenGUI<GUIDice>
            (GameManager.Instance.MatchInfo.MatchDice);

        int seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        gui.SetSeed(seed);

        gui.SetCallback(callback);

        return gui;

    }
}
