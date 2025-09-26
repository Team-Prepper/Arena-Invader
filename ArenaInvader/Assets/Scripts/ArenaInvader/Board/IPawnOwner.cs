using System.Collections.Generic;

public interface IPawnOwner
{
    public IList<GamePawn> Pawns { get; }

    public void SetCC(IPlayableCharacter cc);
    public void SetInitial(string key);

    public void MovePawn(int pawnId, int amount);
    
    public void LeavePawn(int id);
    public void BackHomePawn(int id);

    public void OnPawnChoose();
    public void OffPawnChoose();
}