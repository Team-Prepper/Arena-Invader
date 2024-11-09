using EHTool.UIKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : BasePlayer
{
    
    public override void StartTurn()
    {
        base.StartTurn();
        if(items.Count > 0)
        {
            IItem item = items[Random.Range(0, items.Count)];
            item.UseItem(this);
            items.Remove(item);
        }
        RollDice();
    }
    public override void RollDice()
    {
        _chance--;

        GUIDice guiDice = GameManager.Instance.Playground.GetMatchDice();
        guiDice.SetCallback((value) => {
            Policy(value + extraDicePoint).Move(value + extraDicePoint);
            extraDicePoint = 0;
            guiDice.Close();
        });
        
        guiDice.Roll();
    }

    private Pawn Policy(int value)
    {
        Pawn mostValuablePawn = null;
        int mostValuablePawnValue = -1;

        foreach (var pawn in _pawns)
        {
            if(pawn.IsPiggyBacked()) continue;
            IPlate plate = pawn.MovePredict(value);
            int currentPawnValue = plate == null ? 100 : plate.GetValue(this, SetTarget());
            if(pawn.isPiggied()) currentPawnValue *= 2;
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
            if (player == this) continue;
            int currentHealth = player.GetHealth();
            if (minHealth > currentHealth)
            {
                minHealth = currentHealth;
                target = player;
            }
        }
        return target;
    }
    
    public override void EnterShop(CallbackMethod callback)
    {
        StartCoroutine(EnterShopSequence(callback));
    }

    private IEnumerator EnterShopSequence(CallbackMethod callback)
    {
        GUIShop shop = UIManager.Instance.OpenGUI<GUIShop>("Shop");

        // 1. shop.EnterShop 호출 후 1초 대기
        shop.EnterShop(this, callback);
        yield return new WaitForSeconds(1f);

        // 2. shop.SelectItem 호출 후 1초 대기
        IItem selected = SelectBuyItem(shop.GetSaleItems());
        shop.SelectItem(selected);
        yield return new WaitForSeconds(1f);

        // 3. BuyItem 호출 후 1초 대기
        BuyItem(selected);
        yield return new WaitForSeconds(1f);

        // 4. shop.Close 호출
        shop?.Close();
    }


    private IItem SelectBuyItem(List<IItem> items)
    {
        IItem mostValuableItem = null;
        int mostValuableItemValue = -1;

        foreach (var item in items)
        {
            int currentItemValue = item.ItemValue;
            if (currentItemValue >= mostValuableItemValue && item.Price <= Money)
            {
                mostValuableItem = item;
                mostValuableItemValue = currentItemValue;
            }
        }

        return mostValuableItem;
    }
    
}