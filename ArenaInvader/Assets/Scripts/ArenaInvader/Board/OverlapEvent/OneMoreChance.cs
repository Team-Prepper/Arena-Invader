
using EasyH.Gaming.PathBased;

public class OneMoreChance : IOverlapEvent
{
    public void Event(Plate plate,
        GamePawn defaultPawn, GamePawn newPawn)
    {
        newPawn.GetCC().AddChance();
    }
}
