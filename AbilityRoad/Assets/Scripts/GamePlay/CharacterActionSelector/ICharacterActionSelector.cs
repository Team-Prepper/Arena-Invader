using System;

public interface ICharacterActionSelector {

    public void StartTurn(ICharacterController target);

    public void RollDice();

    public void Inventory(GUIOpenInventory inventory);
    public void Shop(GUIShop shop);

}