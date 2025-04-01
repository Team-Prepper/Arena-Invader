using EHTool.LangKit;
using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GUIBattle : GUINetworkPopUp<int>
{

    [SerializeField] Transform _axis;
    [SerializeField] Text _message;

    [SerializeField] Transform _damageTr;
    [SerializeField] Text _damageTarget;
    [SerializeField] string _damageFormat = "{0}";

    [SerializeField] protected Image _attackerImg;
    [SerializeField] protected Image _targetImg;

    private IStatus _attacker;
    private IStatus _target;

    Action _callback;

    public void BattleSet(int attackerId, int targetId)
    {
        _attacker = GetStatus(attackerId);
        _target = GetStatus(targetId);

        _attackerImg.sprite = CharacterManager.Instance.GetCharacterSprites(_attacker.CharacterCode).CharacterStand;
        _targetImg.sprite = CharacterManager.Instance.GetCharacterSprites(_target.CharacterCode).CharacterStand;

    }

    private IStatus GetStatus(int idx) {
        if (idx < 0) return GameManager.Instance.Playground.ObjectCharacter;
        return GameManager.Instance.Playground.Players[idx].Status;
    }

    public void StartBattle(Action callback)
    {
        _callback = callback;
        PlaySequence(0);
    }

    private void PlaySequence(int idx)
    {
        PlaySequenceAction(idx);
        NetworkModify(idx);
    }

    private void PlaySequenceAction(int idx)
    {

        if (idx < 0)
        {
            _callback?.Invoke();
            TryClose();
            return;
        }

        if (idx == 0)
        {
            WaitASeconds(() => PlaySequence(1));
            return;
        }

        if (idx == 1)
        {
            AttackerAttack();
            return;
        }

        if (idx == 2)
        {
            WaitASeconds(() => PlaySequence(3));
            return;
        }

        TargetAttack();

    }

    private void AttackerAttack()
    {

        AttackSequence(_attackerImg, _targetImg, _attacker, _target, 0, (damage) =>
        {
            _target.HP -= damage;

            if (!_target.IsAlive())
            {
                PlaySequence(-1);
                return;
            }

            PlaySequence(2);

        });

    }

    private void TargetAttack()
    {

        AttackSequence(_targetImg, _attackerImg, _target, _attacker, 1, (damage) =>
        {
            _attacker.HP -= damage;
            WaitASeconds(() => PlaySequence(-2));
        });

    }

    public override void NetworkModifiedEvent(int value)
    {
        if (IsControlled) return;
        if (value % 2 == 0) return;

        PlaySequence(value);
    }

    protected void AttackSequence(Image attackerImg, Image targetImg, IStatus attacker, IStatus target, int attackSequence, Action<int> callback)
    {
        _message.text = string.Format(LangManager.Instance.GetStringByKey("msg_XAttack"), attacker.Name);

        attackerImg.sprite = CharacterManager.Instance.GetCharacterSprites(attacker.CharacterCode).CharacterAttack;
        targetImg.sprite = CharacterManager.Instance.GetCharacterSprites(target.CharacterCode).CharacterDamage;

        SwordAuraController.Instance.SetSwordAura(attackSequence);

        int damage = GameManager.Instance.Playground.CalcDamage(attacker, target);

        _damageTarget.text = string.Format(_damageFormat, damage);
        _damageTr.position = targetImg.transform.position;

        callback?.Invoke(damage);

    }

    protected void WaitASeconds(Action callback)
    {
        StartCoroutine(WaitASecondsSequence(callback));
    }

    protected IEnumerator WaitASecondsSequence(Action callback)
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