public class BackHome : IOverlapEvent {
    public void Event(IPlate plate, Pawn newPawn)
    {
        Pawn defaultPawn = plate.GetPawn();

        defaultPawn.BackHome();

        plate.SetPawn(newPawn);
        newPawn.GetOwner().EndTurn();

    }
}