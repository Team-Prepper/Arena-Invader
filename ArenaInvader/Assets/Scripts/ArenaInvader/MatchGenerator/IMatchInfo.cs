using System;
using System.Collections.Generic;
using EasyH.Unity.UI;

[Serializable]
public struct PlayerInfor {
    public string Name;
    public string CharacterCode;
    public bool IsAI;

    public PlayerInfor(string n, string c = "Player", bool a = false) {
        CharacterCode = c;
        Name = n;
        IsAI = a;
    }

}

public interface IMatchInfo
{

    public IList<int> EditableIdx { get; }
    public IList<PlayerInfor> PlayerInfors { get; }

    public string MapName { get; }
    public string MatchDice { get; }

    public void SetMatchSettingUI(IGUI gui);
    public void StartMatch();
    public void Dispose();

    public void SetPlayerCnt(int cnt);
    public void SetDice(string diceCode);
    public void SetPlayerName(int idx, string name);
    public void SetPlayerCharacter(int idx, string name);
    public void SetPlayerIsAI(int idx, bool isAI);
    public void SetMap(string mapName);
    void OpenSettingUI();
}
