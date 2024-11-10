using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayground
{
    public IList<BasePlayer> Players { get; }
    public BasePlayer NowPlayer { get; }
    public Map Map { get; set; }
    public int Turn { get; }

    public void AddPlayer(BasePlayer player);
    public void PlayerDeath(BasePlayer player);
    public void TurnStart();
    public void TurnEnd();
    public int CalcDamage(Character attacker, Character target);

    public void SetMatchDice(string code);
    public GUIDice GetMatchDice();
    bool IsGameEnd();
}