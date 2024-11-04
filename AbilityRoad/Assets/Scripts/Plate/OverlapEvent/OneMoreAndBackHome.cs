using EHTool.UIKit;

public class OneMoreAndBackHome : IOverlapEvent {
    public void Event(IPlate plate, Pawn newPawn)
    {
        Pawn defaultPawn = plate.GetPawn();

        defaultPawn.BackHome();

        plate.SetPawn(newPawn);
        newPawn.GetOwner().AddChance();
        newPawn.GetOwner().EndTurn();

    }
}