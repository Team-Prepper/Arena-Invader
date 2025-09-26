using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterController : MonoBehaviour, ICharacterController {

    private IPlayableCharacter _target;

    public void StartTurn(IPlayableCharacter target)
    {
        _target = target;

        if (_target.Inventory.Items.Count > 0)
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
        GUIInventory inventory = _target.OpenInventory();
        inventory.SetIsNotControlled();

        int idx = Random.Range(0, _target.Inventory.Items.Count);

        yield return new WaitForSeconds(1f);
        inventory.SelectItem(idx);

        yield return new WaitForSeconds(1f);
        inventory.UseItem();

        yield return new WaitForSeconds(2f);

        inventory.Close();
        RollDice();

    }

    public void RollDice()
    {

        GUIDice guiDice = _target.OpenRollDice((value) => {
            StartCoroutine(PawnSelectSequence(value));
        });
        guiDice.SetIsNotControlled();
        guiDice.Roll();

    }

    
    IEnumerator PawnSelectSequence(int value)
    {
        int idx = Policy(value);

        GUISelectMovePawn movePawn = _target.OpenSelectMovePawn(value);
        movePawn.SetIsNotControlled();

        yield return new WaitForSeconds(1f);
        movePawn.FocusPawn(idx);

        yield return new WaitForSeconds(1f);
        
        movePawn.MovePawn(idx);
        movePawn.Close();
    }

    private int Policy(int value)
    {
        GamePawn mostValuablePawn = null;
        int mostValuablePawnValue = -1;

        foreach (var pawn in _target.PawnOwner.Pawns)
        {
            int pawnValue = pawn.Value();

            if (pawnValue == 0) continue;

            GamePlate plate = pawn.MovePredict(value) as GamePlate;
            int currentPawnValue = plate == null ? 100 : plate.GetValue(_target.Status, SetTarget());

            currentPawnValue *= pawnValue;
            
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
            if (player == _target) continue;
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

    private int SelectBuyItem(IList<string> items)
    {
        int mostValueableItemIdx = -1;
        int mostValuableItemValue = -1;

        for (int i = 0; i < items.Count; i++)
        {
            ItemData itemData = ItemManager.Instance.GetItemData(items[i]);

            if (itemData.ItemValue >= mostValuableItemValue &&
                itemData.Price <= _target.Status.Money)
            {
                mostValueableItemIdx = i;
                mostValuableItemValue = itemData.ItemValue;
            }
        }

        return mostValueableItemIdx;
    }

}