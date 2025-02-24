using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoardGame;

public class AICharacterActionSelector : MonoBehaviour, ICharacterActionSelector {

    ICharacterController _target;

    public void StartTurn(ICharacterController target)
    {
        _target = target;

        if (_target.Status.Items.Count > 0)
        {
            Inventory();
            return;
        }

        RollDice();
    }

    public void Inventory()
    {
        StartCoroutine(InventorySequence());

    }

    IEnumerator InventorySequence()
    {
        GUIOpenInventory inventory = _target.OpenInventory();
        inventory.SetIsNotControlled();

        int idx = Random.Range(0, _target.Status.Items.Count);

        Debug.Log(idx);

        yield return new WaitForSeconds(1f);
        inventory.SelectItem(idx);

        yield return new WaitForSeconds(1f);
        inventory.UseItem(idx);

        yield return new WaitForSeconds(1f);

        inventory.Close();
        RollDice();

    }

    public void RollDice()
    {

        GUIDice guiDice = _target.RollDice((value) => {
            StartCoroutine(PawnSelectSequence(value));
        });
        guiDice.SetIsNotControlled();
        guiDice.Roll();

    }

    
    IEnumerator PawnSelectSequence(int value)
    {
        int idx = Policy(value);

        GUISelectMovePawn movePawn = _target.SelectMovePawn(value);
        movePawn.SetIsNotControlled();

        yield return new WaitForSeconds(1f);
        movePawn.FocusPawn(idx);

        yield return new WaitForSeconds(1f);
        movePawn.MovePawn(idx);

    }

    private int Policy(int value)
    {
        Pawn mostValuablePawn = null;
        int mostValuablePawnValue = -1;

        foreach (var pawn in _target.Target.Pawns)
        {
            if (pawn.IsPiggyBacked()) continue;

            Plate plate = pawn.MovePredict(value);
            int currentPawnValue = 100;//plate == null ? 100 : plate.GetValue(_target.Status, SetTarget());
            
            if (pawn.IsPiggied()) currentPawnValue *= 2;
            
            if (currentPawnValue >= mostValuablePawnValue)
            {
                mostValuablePawn = pawn;
                mostValuablePawnValue = currentPawnValue;
            }

        }

        return mostValuablePawn.Id;

    }

    private IStatus SetTarget()
    {
        IStatus target = null;
        int minHealth = int.MaxValue;

        foreach (var player in GameManager.Instance.Playground.Players)
        {
            if (player.Target == _target.Target) continue;
            int currentHealth = player.Status.HP;
            if (minHealth > currentHealth)
            {
                minHealth = currentHealth;
                target = player.Status;
            }
        }
        
        return target;
    }

    public void Shop(GUIShop shop)
    {
        shop.SetIsNotControlled();
        StartCoroutine(ShopSequence(shop));
    }

    private IEnumerator ShopSequence(GUIShop shop)
    {
        yield return new WaitForSeconds(1f);

        int selected = SelectBuyItem(shop.GetSaleItems());

        if (selected == -1)
        {
            shop?.Close();
            yield break;
        }

        shop.SelectItem(selected);
        yield return new WaitForSeconds(1f);

        shop.Buy();
        yield return new WaitForSeconds(1f);

        shop?.Close();

    }

    private int SelectBuyItem(IList<ItemData> items)
    {
        int mostValueableItemIdx = -1;
        int mostValuableItemValue = -1;

        for (int i = 0; i < items.Count; i++)
        {
            int currentItemValue = items[i].ItemValue;
            if (currentItemValue >= mostValuableItemValue && items[i].Price <= _target.Status.Money)
            {
                mostValueableItemIdx = i;
                mostValuableItemValue = currentItemValue;
            }
        }

        return mostValueableItemIdx;
    }

}