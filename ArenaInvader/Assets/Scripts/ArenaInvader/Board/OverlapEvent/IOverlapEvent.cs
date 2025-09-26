using EasyH.Gaming.PathBased;

public interface IOverlapEvent
{
    public void Event(Plate plate,
        GamePawn defaultPawn, GamePawn newPawn);
}