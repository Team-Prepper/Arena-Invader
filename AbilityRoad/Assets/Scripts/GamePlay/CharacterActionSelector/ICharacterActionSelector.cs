using System;

public interface ICharacterActionSelector {

    public void StartTurn(ICharacterController target);

    public void RollDice();

    public void Inventory();

    public void Shop(GUIShop shop);

}