public interface ICharacterController {

    public void StartTurn(IPlayableCharacter target);

    public void RollDice();

    public void Inventory();

    public void Shop(GUIShop shop);
    
}