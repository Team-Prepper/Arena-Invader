using System;

public interface ICharacterActionSelector {

    public void StartTurn(ICharacterController target);

    public void RollDice();

    public void SelectItem(GUIShop shop, Action<int> callback);

}