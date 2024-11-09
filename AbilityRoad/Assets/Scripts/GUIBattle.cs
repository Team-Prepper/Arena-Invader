using EHTool.UIKit;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GUIBattle : GUIPopUp {

    [SerializeField] Text _message;

    public void StartBattle(Character attacker, CallbackMethod callback) {

        SetTarget(attacker, (target) => {

            StartCoroutine(WaitASeconds(() => {

                target.ReduceHealth(GameManager.Instance.Playground.CalcDamage(attacker, target));

                if (!target.IsAlive())
                {
                    StartCoroutine(WaitASeconds(() => {
                        callback?.Invoke();
                        Close();
                    }));
                    return;
                }

                StartCoroutine(WaitASeconds(() => {
                    attacker.ReduceHealth(GameManager.Instance.Playground.CalcDamage(target, attacker));

                    StartCoroutine(WaitASeconds(() => {
                        callback?.Invoke();
                        Close();
                    }));

                }));

            }));

        });
        
    }

    public void SetTarget(Character attacker, CallbackMethod<Character> callback) {

        foreach (var player in GameManager.Instance.Playground.Players)
        {
            if (player == attacker) continue;
            callback?.Invoke(player);
            return;
        }
    }

    IEnumerator WaitASeconds(CallbackMethod callback) {
        yield return new WaitForSeconds(1f);

        callback?.Invoke();
    }
}
