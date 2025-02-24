using System;

public class GUIRaid : GUIBattle {

    public void StartRaid(BasePlayer attacker, ObjectCharacter target, Action callback) {
        
        /*
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

                    StartCoroutine(WaitASeconds(() =>
                    {
                        callback?.Invoke();
                        Close();
                    }));
                });

            });
        }));
        */
    }

}
