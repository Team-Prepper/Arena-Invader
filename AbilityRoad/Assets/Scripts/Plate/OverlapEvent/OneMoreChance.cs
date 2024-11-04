using EHTool.UIKit;

public class OneMoreChance : IOverlapEvent {
    public void Event(IPlate plate, Pawn newPawn)
    {
        Pawn defaultPawn = plate.GetPawn();

        UIManager.Instance.OpenGUI<GUIDice>("Dice").SetCallback((amount) =>
        {
            newPawn.Move(amount);
        });
    }
}