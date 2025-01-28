public struct LocalMatchInfor : MatchInfor {

    MatchInfor.PlayerInfor[] MatchInfor.PlayerInfors
        => _playerInfors;

    string MatchInfor.MapName
        => _mapName;

    string MatchInfor.MatchDice
        => _matchDice;

    public MatchInfor.PlayerInfor[] _playerInfors;
    public string _mapName;
    public string _matchDice;

    public LocalMatchInfor(int cnt = 2, string mapName = "Map/DefaultMap", string matchDice = "DartDice")
    {
        _playerInfors = new MatchInfor.PlayerInfor[cnt];

        for (int i = 0; i < cnt; i++)
        {
            _playerInfors[i] = new MatchInfor.PlayerInfor();
        }

        _mapName = mapName;
        _matchDice = matchDice;
    }

    public void SetPlayerCnt(int cnt)
    {
        int defaultCnt = _playerInfors.Length;

        MatchInfor.PlayerInfor[] newInfor = new MatchInfor.PlayerInfor[cnt];

        for (int i = 0; i < cnt; i++)
        {
            if (i >= defaultCnt)
            {
                newInfor[i] = new MatchInfor.PlayerInfor();
                continue;
            }
            newInfor[i] = _playerInfors[i];
        }

        _playerInfors = newInfor;
    }

    public void SetDice(string diceCode) {
        _matchDice = diceCode;
    }

    public void SetPlayerName(int idx, string name)
    {
        if (idx >= _playerInfors.Length) return;
        _playerInfors[idx].Name = name;
    }

    public void SetPlayerCharacter(int idx, string name)
    {
        if (idx >= _playerInfors.Length) return;
        _playerInfors[idx].CharacterCode = name;
    }

    public void SetMap(string mapName) {
        _mapName = mapName;
    }
}
