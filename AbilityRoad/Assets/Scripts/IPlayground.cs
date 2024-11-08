using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayground
{
    public IList<BasePlayer> Players { get; }
    public Map Map { get; set; }
    public int Turn { get; }

    public void AddPlayer(BasePlayer player);
    public void TurnStart();
    public void TurnEnd();

}
