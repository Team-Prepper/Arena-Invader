using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterActionSelector : ICharacterActionSelector {

    ICharacterController _target;

    public void StartTurn(ICharacterController target)
    {
        _target = target;

        if (_target.Target.items.Count > 0)
        {
            IItem item = _target.Target.items[UnityEngine.Random.Range(0, _target.Target.items.Count)];
            item.UseItem(_target);
            _target.Target.items.Remove(item);
        }

        RollDice();
    }

    public void RollDice()
    {

        GUIDice guiDice = _target.RollDice((value) => {
            Policy(value).Move(value);
        });

        guiDice.Roll();

    }

    private Pawn Policy(int value)
    {
        Pawn mostValuablePawn = null;
        int mostValuablePawnValue = -1;

        foreach (var pawn in _target.Target._pawns)
        {
            if (pawn.IsPiggyBacked()) continue;

            IPlate plate = pawn.MovePredict(value);
            int currentPawnValue = plate == null ? 100 : plate.GetValue(_target.Target, SetTarget());
            
            if (pawn.isPiggied()) currentPawnValue *= 2;
            
            if (currentPawnValue >= mostValuablePawnValue)
            {
                mostValuablePawn = pawn;
                mostValuablePawnValue = currentPawnValue;
            }

        }

        return mostValuablePawn;

    }

    private Character SetTarget()
    {
        Character target = null;
        int minHealth = int.MaxValue;
        foreach (var player in GameManager.Instance.Playground.Players)
        {
            if (player.Target == _target.Target) continue;
            int currentHealth = player.Target.GetHealth();
            if (minHealth > currentHealth)
            {
                minHealth = currentHealth;
                target = player.Target;
            }
        }
        return target;
    }

    public void SelectItem(GUIShop shop, Action<int> callback)
    {
        shop?.CloseButton();
        //StartCoroutine(EnterShopSequence(shop));
        //callback?.Invoke(1);
    }

    private IEnumerator EnterShopSequence(GUIShop shop)
    {

        // 1. shop.EnterShop 호출 후 1초 대기
        yield return new WaitForSeconds(1f);

        // 2. shop.SelectItem 호출 후 1초 대기
        IItem selected = SelectBuyItem(shop.GetSaleItems());
        shop.SelectItem(selected);
        yield return new WaitForSeconds(1f);

        // 3. BuyItem 호출 후 1초 대기
        _target.Target.BuyItem(selected);
        yield return new WaitForSeconds(1f);

        // 4. shop.Close 호출
        shop?.CloseButton();
    }

    private IItem SelectBuyItem(List<IItem> items)
    {
        IItem mostValuableItem = null;
        int mostValuableItemValue = -1;

        foreach (var item in items)
        {
            int currentItemValue = item.ItemValue;
            if (currentItemValue >= mostValuableItemValue && item.Price <= _target.Target.Money)
            {
                mostValuableItem = item;
                mostValuableItemValue = currentItemValue;
            }
        }

        return mostValuableItem;
    }

}