using System.Collections.Generic;
using UnityEngine;

public interface IPlayground {
    public GameMap Map { get; set; }
    public int Turn { get; }

    public IList<ICharacterController> Players { get; }
    public ICharacterController NowPlayer { get; }
    public ICharacterController InstantiateCC(Vector3 position);
    
    public IStatus InstantiateStatus();
    public IStatus ObjectCharacter { get; set; }

    void StartMatch();

    public void PlayReady();

    public void AddPlayer(ICharacterController player);
    public void PlayerDeath(ICharacterController player);
    public void TurnStart();
    public void TurnEnd();

    public int CalcDamage(IStatus attacker, IStatus target);
    public bool IsGameEnd();
}