using EHTool.UIKit;

public class OneMoreChance : IOverlapEvent {
    public void Event(IPlate plate, Pawn newPawn)
    {
        Pawn defaultPawn = plate.GetPawn();

        GameManager.Instance.Playground.GetMatchDice().SetCallback((amount) =>
        {
            newPawn.Move(amount);
        });
    }
}