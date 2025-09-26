using EasyH.Gaming.PathBased;

public class BackHome : IOverlapEvent
{
    public void Event(Plate plate,
        GamePawn defaultPawn, GamePawn newPawn)
    {
        defaultPawn.Killed();
        BoardManager.Instance.SetPawnAt(plate, newPawn);

    }
}