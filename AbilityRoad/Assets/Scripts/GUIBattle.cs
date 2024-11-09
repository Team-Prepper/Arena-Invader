using EHTool.UIKit;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GUIBattle : GUIPopUp {

    [SerializeField] Transform _axis;
    [SerializeField] Text _message;

    [SerializeField] Transform _damageTr;
    [SerializeField] Text _damageTarget;
    [SerializeField] string _damageFormat = "{0}";

    [SerializeField] Image _attacker;
    [SerializeField] Image _target;


    public void StartBattle(Character attacker, CallbackMethod callback)
    {

        SetTarget(attacker, (target) =>
        {

            _attacker.sprite = CharacterManager.Instance.GetPlayerStandSpr(attacker.GetCharacterCode());
            _target.sprite = CharacterManager.Instance.GetPlayerStandSpr(target.GetCharacterCode());

            StartCoroutine(WaitASeconds(() =>
            {

                AttackSequence(_attacker, _target, attacker, target, () =>
                {

                    if (!target.IsAlive())
                    {
                        callback?.Invoke();
                        Close();
                        return;
                    }

                    AttackSequence(_target, _attacker, target, attacker, () =>
                    {
                        StartCoroutine(WaitASeconds(() =>
                        {
                            callback?.Invoke();
                            Close();
                        }));
                    });

                });
            }));


        });

    }

    void AttackSequence(Image attackerImg, Image targetImg, Character attacker, Character target, CallbackMethod callback)
    {
        _message.text = string.Format("{0}ÀÇ °ø°Ý!!", attacker.GetName());

        attackerImg.sprite = CharacterManager.Instance.GetPlayerAttackerSpr(attacker.GetCharacterCode());
        targetImg.sprite = CharacterManager.Instance.GetPlayerDamageSpr(target.GetCharacterCode());

        int damage = GameManager.Instance.Playground.CalcDamage(attacker, target);

        target.ReduceHealth(damage);

        _damageTarget.text = string.Format(_damageFormat, damage);
        _damageTr.position = targetImg.transform.position;

        StartCoroutine(WaitASeconds(() =>
        {
            callback?.Invoke();
        }));

    }

    public void SetTarget(Character attacker, CallbackMethod<Character> callback)
    {

        foreach (var player in GameManager.Instance.Playground.Players)
        {
            if (player == attacker) continue;
            callback?.Invoke(player);
            return;
        }
    }

    IEnumerator WaitASeconds(CallbackMethod callback)
    {
        yield return new WaitForSeconds(1f);

        callback?.Invoke();
    }
}
