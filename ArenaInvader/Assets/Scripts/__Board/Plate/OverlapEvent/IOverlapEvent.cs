namespace BoardGame
{

    public interface IOverlapEvent
    {
        public void Event(Plate plate, Pawn newPawn);
    }

}