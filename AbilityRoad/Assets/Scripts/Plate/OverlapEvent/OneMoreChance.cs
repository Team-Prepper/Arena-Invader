using EHTool.UIKit;

public class OneMoreChance : IOverlapEvent {
    public void Event(Pawn defaultPawn, Pawn newPawn)
    {
        if (defaultPawn.GetOwner() == newPawn.GetOwner()) {
            defaultPawn.PiggyBack(newPawn);
            defaultPawn.GetOwner().EndTurn();
            return;
        }

        UIManager.Instance.OpenGUI<GUIDice>("Dice").SetCallback((amount) =>
        {
            newPawn.Move(amount);
        });
    }
}