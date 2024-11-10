using EHTool.LangKit;
using EHTool.UIKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIRaid : GUIPopUp {
    
    [SerializeField] Transform _axis;
    [SerializeField] Text _message;

    [SerializeField] Transform _damageTr;
    [SerializeField] Text _damageTarget;
    [SerializeField] string _damageFormat = "{0}";

    [SerializeField] Image _attacker;
    [SerializeField] Image _target;

    public void StartRaid(BasePlayer attacker, ObjectCharacter target, CallbackMethod callback) {
        
        _attacker.sprite = CharacterManager.Instance.GetPlayerStandSpr(attacker.GetCharacterCode());
        _target.sprite = CharacterManager.Instance.GetPlayerStandSpr(target.GetCharacterCode());

        StartCoroutine(WaitASeconds(() =>
        {

            AttackSequence(_attacker, _target, attacker, target, 0, (int damage) =>
            {
                target.ReduceHealth(damage);

                if (!target.IsAlive())
                {
                    attacker.SlainObject();
                    callback?.Invoke();
                    Close();
                    return;
                }

                AttackSequence(_target, _attacker, target, attacker, 1, (damage) =>
                {
                    attacker.ReduceHealth(damage);

                    if (!attacker.IsAlive())
                    {
                        callback?.Invoke();
                        Close();
                    }

                    StartCoroutine(WaitASeconds(() =>
                    {
                        callback?.Invoke();
                        Close();
                    }));
                });

            });
        }));
    }
    
    void AttackSequence(Image attackerImg, Image targetImg, Character attacker, Character target, int attackSequence, CallbackMethod<int> callback)
    {
        _message.text = string.Format(LangManager.Instance.GetStringByKey("msg_XAttack"), attacker.GetName());

        attackerImg.sprite = CharacterManager.Instance.GetPlayerAttackerSpr(attacker.GetCharacterCode());
        targetImg.sprite = CharacterManager.Instance.GetPlayerDamageSpr(target.GetCharacterCode());

        SwordAuraController.Instance.SetSwordAura(attackSequence);
        
        int damage = GameManager.Instance.Playground.CalcDamage(attacker, target);

        _damageTarget.text = string.Format(_damageFormat, damage);
        _damageTr.position = targetImg.transform.position;

        StartCoroutine(WaitASeconds(() =>
        {
            callback?.Invoke(damage);
        }));

    }

    IEnumerator WaitASeconds(CallbackMethod callback)
    {
        yield return new WaitForSeconds(1f);

        callback?.Invoke();
    }
}
