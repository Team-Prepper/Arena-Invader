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
            int currentPawnValue = plate == null ? 10 : plate.GetValue(this, SetTarget());
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
        GUIShop shop = UIManager.Instance.OpenGUI<GUIShop>("Shop");
        shop.EnterShop(this, callback);
        IItem selected = SelectBuyItem(shop.GetSaleItems());
        shop.SelectItem(selected);
        BuyItem(selected);
        shop.Close();
    }

    private IItem SelectBuyItem(List<IItem> items)
    {
        IItem mostValuableItem = null;
        int mostValuableItemValue = -1;

        foreach (var item in items)
        {
            int currentItemValue = item.ItemValue;
            if (currentItemValue >= mostValuableItemValue && item.Price <= money)
            {
                mostValuableItem = item;
                mostValuableItemValue = currentItemValue;
            }
        }

        return mostValuableItem;
    }
    
}