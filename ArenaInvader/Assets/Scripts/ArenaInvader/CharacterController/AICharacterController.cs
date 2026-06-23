using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyH.Unity.UI;

public class AICharacterController : MonoBehaviour, ICharacterController {
    private const float ActionDelaySeconds = 1f;
    private const float DiceFollowUpDelaySeconds = 1f;

    private IPlayableCharacter _target;
    private GUIPlayerAction _actionUI;

    public void StartTurn(IPlayableCharacter target)
    {
        _target = target;
        StopAllCoroutines();
        OpenActionUI();
        StartCoroutine(StartTurnSequence());
    }

    private IEnumerator StartTurnSequence()
    {
        yield return new WaitForSeconds(ActionDelaySeconds);

        if (_target.Inventory.Items.Count > 0)
        {
            Inventory();
            yield break;
        }

        RollDice();
    }

    public void Inventory()
    {
        CloseActionUI();
        StartCoroutine(InventorySequence());

    }

    IEnumerator InventorySequence()
    {
        GUIInventory inventory = _target.OpenInventory();
        inventory.SetIsNotControlled();

        int idx = Random.Range(0, _target.Inventory.Items.Count);

        yield return new WaitForSeconds(ActionDelaySeconds);
        inventory.SelectItem(idx);

        yield return new WaitForSeconds(ActionDelaySeconds);
        inventory.UseItem();

        yield return new WaitForSeconds(ActionDelaySeconds);

        inventory.Close();
        yield return new WaitForSeconds(ActionDelaySeconds);
        RollDice();

    }

    public void RollDice()
    {
        CloseActionUI();
        StartCoroutine(RollDiceSequence());
    }

    private IEnumerator RollDiceSequence()
    {
        GUIDice guiDice = _target.OpenRollDice((value) => {
            StartCoroutine(PawnSelectSequence(value));
        });
        guiDice.SetIsNotControlled();

        yield return new WaitForSeconds(ActionDelaySeconds);
        guiDice.Roll();
    }
    
    IEnumerator PawnSelectSequence(int value)
    {
        int idx = Policy(value);

        GUISelectMovePawn movePawn = _target.OpenSelectMovePawn(value);
        movePawn.SetIsNotControlled();

        yield return new WaitForSeconds(DiceFollowUpDelaySeconds);
        movePawn.FocusPawn(idx);

        yield return new WaitForSeconds(ActionDelaySeconds);
        
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
        yield return new WaitForSeconds(ActionDelaySeconds);

        int selected = SelectBuyItem(shop.GetSaleItems());

        if (selected == -1)
        {
            shop?.Close();
            yield break;
        }

        shop.SelectItem(selected);
        yield return new WaitForSeconds(ActionDelaySeconds);

        shop.Buy();
        yield return new WaitForSeconds(ActionDelaySeconds);

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

    private void OpenActionUI()
    {
        CloseActionUI();

        _actionUI = UIManager.Instance.OpenGUI<GUIPlayerAction>("PlayerAction");
        _actionUI.PlayerTurnStart(this);
        _actionUI.SetIsNotControlled();
    }

    private void CloseActionUI()
    {
        if (_actionUI == null)
        {
            return;
        }

        _actionUI.Close();
        _actionUI = null;
    }

}
