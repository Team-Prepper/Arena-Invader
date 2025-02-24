
namespace BoardGame
{
    public class OneMoreChance : IOverlapEvent
    {
        public void Event(Plate plate, Pawn newPawn)
        {
            newPawn.GetPlayer().AddChance();
            newPawn.GetPlayer().EndTurn();
        }
    }
    
}