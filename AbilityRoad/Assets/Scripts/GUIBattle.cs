using EHTool.UIKit;
using System.Collections;
using UnityEngine;

public class GUIBattle : GUIPopUp {
    public void StartBattle(Player attacker, CallbackMethod callback) {

        foreach (var player in GameManager.Instance.Playground.Players) {
            foreach (var victim in GameManager.Instance.Playground.Players) {
                if (player == victim) continue;
            }
        }

        StartCoroutine(WaitASeconds(callback));
        
    }

    IEnumerator WaitASeconds(CallbackMethod callback) {
        yield return new WaitForSeconds(1f);

        callback?.Invoke();
        Close();
    }
}
