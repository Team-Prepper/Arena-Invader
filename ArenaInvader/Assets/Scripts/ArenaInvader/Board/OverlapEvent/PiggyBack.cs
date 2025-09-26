using EasyH.Gaming.PathBased;

public class PiggyBack : IOverlapEvent
{
    public void Event(Plate plate,
        GamePawn defaultPawn, GamePawn newPawn)
    {
        defaultPawn.PiggyBack(newPawn);
    }
}