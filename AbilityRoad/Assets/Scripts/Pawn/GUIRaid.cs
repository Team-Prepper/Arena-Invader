using EHTool.UIKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIRaid : GUIFullScreen {

    public void StartRaid(BasePlayer attacker, ObjectCharacter target, CallbackMethod callback) {
        
        StartCoroutine(WaitASeconds(() => {
            target.ReduceHealth(GameManager.Instance.Playground.CalcDamage(attacker, target));
            if (!target.IsAlive()) {
                target.RewardTo(attacker);

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
    }

    IEnumerator WaitASeconds(CallbackMethod callback)
    {
        yield return new WaitForSeconds(1f);

        callback?.Invoke();
    }
}
