using EHTool.UIKit;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GUIBattle : GUIPopUp {

    [SerializeField] Text _message;

    public void StartBattle(Character attacker, CallbackMethod callback) {

        SetTarget(attacker, (target) => {

            StartCoroutine(WaitASeconds(() => {

                target.ReduceHealth(CalcDamage(attacker, target));

                StartCoroutine(WaitASeconds(() => {
                    attacker.ReduceHealth(CalcDamage(target, attacker));

                    StartCoroutine(WaitASeconds(() => {
                        callback?.Invoke();
                        Close();
                    }));

                }));

            }));

        });
        
    }

    int CalcDamage(Character attacker, Character target)
    {
        return Mathf.Max(1, attacker.GetAttackValue() - target.GetDefenseValue());

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
