using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MatchInfor
{
    [System.Serializable]
    public class PlayerInfor {
        public string CharacterCode = "Player";
        public string Name = "Player";
        
    }

    public PlayerInfor[] PlayerInfors = new PlayerInfor[2];
    public string MapName = "Map/DefaultMap";
    public string MatchDice = "DartDice";

}
