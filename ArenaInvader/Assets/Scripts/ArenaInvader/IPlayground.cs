using System.Collections.Generic;
using UnityEngine;

public interface IPlayground
{
    public IList<IPlayableCharacter> Players { get; }
    public IPlayableCharacter NowPlayer { get; }
    public IStatus ObjectCharacter { get; set; }

    public IPlayableCharacter InstantiateCC(Vector3 position);
    public IPlayableCharacter InstantiateCC(Vector3 position, ulong ownerClientId);

    public IStatus InstantiateStatus();

    public void StartMatch();
    public void MatchLoadComplete();

    public void AddPlayer(IPlayableCharacter player);
    public void PlayerDeath(IPlayableCharacter player);
    public void CheckGameEnd();
    
    public int CalcDamage(IStatus attacker, IStatus target);
    
}
