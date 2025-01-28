using EHTool.LangKit;
using EHTool.UIKit;
using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GUIBattle : GUIPopUp {

    [SerializeField] Transform _axis;
    [SerializeField] Text _message;

    [SerializeField] Transform _damageTr;
    [SerializeField] Text _damageTarget;
    [SerializeField] string _damageFormat = "{0}";

    [SerializeField] protected Image _attacker;
    [SerializeField] protected Image _target;

    public void StartBattle(Character attacker, Character target, Action callback)
    {

        _attacker.sprite = CharacterManager.Instance.GetPlayerStandSpr(attacker.GetCharacterCode());
        _target.sprite = CharacterManager.Instance.GetPlayerStandSpr(target.GetCharacterCode());

        StartCoroutine(WaitASeconds(() =>
        {

            AttackSequence(_attacker, _target, attacker, target, 0, (int damage) =>
            {
                target.ReduceHealth(damage);

                if (!target.IsAlive())
                {
                    callback?.Invoke();
                    Close();
                    return;
                }


                StartCoroutine(WaitASeconds(() =>
                {

                    AttackSequence(_target, _attacker, target, attacker, 1, (damage) =>
                    {
                        attacker.ReduceHealth(damage);

                        StartCoroutine(WaitASeconds(() =>
                        {
                            callback?.Invoke();
                            Close();
                        }));
                    });

                }));

            });
        }));

    }

    protected void AttackSequence(Image attackerImg, Image targetImg, Character attacker, Character target, int attackSequence, Action<int> callback)
    {
        _message.text = string.Format(LangManager.Instance.GetStringByKey("msg_XAttack"), attacker.GetName());

        attackerImg.sprite = CharacterManager.Instance.GetPlayerAttackerSpr(attacker.GetCharacterCode());
        targetImg.sprite = CharacterManager.Instance.GetPlayerDamageSpr(target.GetCharacterCode());

        SwordAuraController.Instance.SetSwordAura(attackSequence);
        
        int damage = GameManager.Instance.Playground.CalcDamage(attacker, target);

        _damageTarget.text = string.Format(_damageFormat, damage);
        _damageTr.position = targetImg.transform.position;

        callback?.Invoke(damage);

    }


    protected IEnumerator WaitASeconds(Action callback)
    {
        yield return new WaitForSeconds(1f);

        callback?.Invoke();
    }

    public override void Close()
    {
        StopAllCoroutines();
        base.Close();
    }
}
