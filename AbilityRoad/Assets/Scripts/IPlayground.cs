using System.Collections.Generic;

public interface IPlayground
{
    public IList<ICharacterController> Players { get; }
    public ICharacterController NowPlayer { get; }
    public ICharacterController InstantiateCC();

    public Map Map { get; set; }
    public int Turn { get; }
    MatchInfor MatchInfor { get; }

    public void AddPlayer(ICharacterController player);
    public void PlayerDeath(ICharacterController player);
    public void TurnStart();
    public void TurnEnd();
    public int CalcDamage(Character attacker, Character target);

    bool IsGameEnd();
}