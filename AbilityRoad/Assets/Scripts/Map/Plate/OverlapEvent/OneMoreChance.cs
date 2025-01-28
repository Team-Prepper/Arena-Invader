using EHTool.UIKit;

public class OneMoreChance : IOverlapEvent {
    public void Event(IPlate plate, Pawn newPawn)
    {
        Pawn defaultPawn = plate.GetPawn();

        newPawn.GetOwner().AddChance();
        newPawn.GetOwner().EndTurn();
    }
}