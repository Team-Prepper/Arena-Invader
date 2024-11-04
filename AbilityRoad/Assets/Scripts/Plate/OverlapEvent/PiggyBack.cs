public class PiggyBack : IOverlapEvent {
    public void Event(IPlate plate, Pawn newPawn)
    {
        Pawn defaultPawn = plate.GetPawn();

        defaultPawn.PiggyBack(newPawn);
        defaultPawn.GetOwner().EndTurn();
    }
}