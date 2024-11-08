using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MatchInfor
{
    [System.Serializable]
    public class PlayerInfor {
        public string CharacterName;
        public string Name;
        
    }

    public PlayerInfor[] PlayerInfors;
    public string MapName;

}
