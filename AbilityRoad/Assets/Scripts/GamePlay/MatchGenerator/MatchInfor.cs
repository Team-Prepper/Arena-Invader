public interface MatchInfor
{
    [System.Serializable]
    public class PlayerInfor {
        public string CharacterCode = "Player";
        public string Name = "Player";
        public bool IsAI = false;

    }

    public PlayerInfor[] PlayerInfors { get; }
    public string MapName { get; }
    public string MatchDice { get; }

    public void SetPlayerCnt(int cnt);
    public void SetDice(string diceCode);
    public void SetPlayerName(int idx, string name);
    public void SetPlayerCharacter(int idx, string name);
    public void SetMap(string mapName);

}
