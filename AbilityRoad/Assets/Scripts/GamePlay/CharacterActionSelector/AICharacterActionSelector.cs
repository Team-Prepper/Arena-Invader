using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AICharacterActionSelector : MonoBehaviour, ICharacterActionSelector {

    ICharacterController _target;

    public void StartTurn(ICharacterController target)
    {
        _target = target;

        if (_target.Target.items.Count > 0)
        {
            StartCoroutine(InventorySequence());
            return;
        }

        RollDice();
    }

    IEnumerator InventorySequence()
    {
        GUIOpenInventory inventory = _target.OpenInventory();
        int idx = UnityEngine.Random.Range(0, _target.Target.items.Count);

        Debug.Log(idx);

        // 1. shop.EnterShop 호출 후 1초 대기
        yield return new WaitForSeconds(1f);
        inventory.SelectItem(idx);

        // 1. shop.EnterShop 호출 후 1초 대기
        yield return new WaitForSeconds(1f);
        inventory.UseItem(idx);

        yield return new WaitForSeconds(1f);

        inventory.TryClose();
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

    public void Inventory(GUIOpenInventory inventory) { 
        
    }

    public void Shop(GUIShop shop)
    {
        StartCoroutine(ShopSequence(shop));
    }

    private IEnumerator ShopSequence(GUIShop shop)
    {

        // 1. shop.EnterShop 호출 후 1초 대기
        yield return new WaitForSeconds(1f);

        // 2. shop.SelectItem 호출 후 1초 대기
        int selected = SelectBuyItem(shop.GetSaleItems());

        if (selected == -1)
        {
            // 4. shop.Close 호출
            shop?.TryClose();
            yield break;
        }

        shop.SelectItem(selected);
        yield return new WaitForSeconds(1f);

        // 3. BuyItem 호출 후 1초 대기
        shop.BuyButton();
        yield return new WaitForSeconds(1f);

        // 4. shop.Close 호출
        shop?.TryClose();

    }

    private int SelectBuyItem(IList<ItemData> items)
    {
        int mostValueableItemIdx = -1;
        int mostValuableItemValue = -1;

        for (int i = 0; i < items.Count; i++)
        {
            int currentItemValue = items[i].ItemValue;
            if (currentItemValue >= mostValuableItemValue && items[i].Price <= _target.Target.Money)
            {
                mostValueableItemIdx = i;
                mostValuableItemValue = currentItemValue;
            }
        }

        return mostValueableItemIdx;
    }

}