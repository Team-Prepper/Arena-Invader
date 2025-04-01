namespace BoardGame
{

    public class PiggyBack : IOverlapEvent
    {

        public void Event(Plate plate, Pawn newPawn)
        {
            Pawn defaultPawn = plate.GetPawn();

            defaultPawn.PiggyBack(newPawn);
        }

    }
}