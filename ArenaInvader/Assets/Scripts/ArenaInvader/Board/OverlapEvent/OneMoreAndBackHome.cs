using EasyH.Gaming.PathBased;
public class OneMoreAndBackHome : IOverlapEvent
{

    public void Event(Plate plate,
        GamePawn defaultPawn, GamePawn newPawn)
    {
        defaultPawn.Killed();
        newPawn.GetCC().AddChance();
        BoardManager.Instance.OccupyPawnAt(plate, newPawn);
    }

}
