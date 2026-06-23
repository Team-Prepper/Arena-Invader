using System.Collections.Generic;
using EasyH.Unity.UI;

[System.Serializable]
public struct MatchInfo : IMatchInfo {

    public IList<PlayerInfor> PlayerInfors
        => _playerInfors;

    public string MapName => _mapName;

    public string MatchDice => _matchDice;

    public IList<int> EditableIdx {
        get { 
            IList<int> retval = new List<int>();
            for (int i = 0; i < PlayerInfors.Count; i++) {
                retval.Add(i);
            }
            return retval;
        }
    }

    public PlayerInfor[] _playerInfors;
    public string _mapName;
    public string _matchDice;

    private IGUI _gui;

    public void OpenSettingUI() {
        UIManager.Instance.OpenGUI<GUIWindow>("LocalMatchInforSetting");
    }

    public void SetMatchSettingUI(IGUI gui) {
        _gui = gui;
    }

    public MatchInfo(int cnt = 2, string mapName = "Map/DefaultMap", string matchDice = "DartDice")
    {
        _playerInfors = new PlayerInfor[cnt];

        for (int i = 0; i < cnt; i++)
        {
            _playerInfors[i] = new PlayerInfor(string.Format("Player {0}", i));
        }

        _mapName = mapName;
        _matchDice = matchDice;

        GameManager.Instance.OnMatchInfoChanged?.Invoke();
        _gui = null;
    }

    public void StartMatch()
    {
        GameManager.Instance.Playground.StartMatch();
    }

    public void Dispose()
    {
        if (_gui == null) return;
        _gui.Close();
    }

    public void SetPlayerCnt(int cnt)
    {
        int defaultCnt = _playerInfors.Length;

        PlayerInfor[] newInfor = new PlayerInfor[cnt];

        for (int i = 0; i < cnt; i++)
        {
            if (i >= defaultCnt)
            {
                newInfor[i] = new PlayerInfor(string.Format("Player {0}", i));
                continue;
            }
            newInfor[i] = _playerInfors[i];
        }

        _playerInfors = newInfor;
        GameManager.Instance.OnMatchInfoChanged?.Invoke();
    }

    public void SetDice(string diceCode) {
        _matchDice = diceCode;
        GameManager.Instance.OnMatchInfoChanged?.Invoke();
    }

    public void SetPlayerName(int idx, string name)
    {
        if (idx >= _playerInfors.Length) return;
        _playerInfors[idx].Name = name;
        GameManager.Instance.OnMatchInfoChanged?.Invoke();
    }

    public void SetPlayerCharacter(int idx, string name)
    {
        if (idx >= _playerInfors.Length) return;
        _playerInfors[idx].CharacterCode = name;
        GameManager.Instance.OnMatchInfoChanged?.Invoke();
    }

    public void SetPlayerIsAI(int idx, bool isAI)
    {
        if (idx >= _playerInfors.Length) return;
        _playerInfors[idx].IsAI = isAI;
        GameManager.Instance.OnMatchInfoChanged?.Invoke();
    }

    public void SetMap(string mapName) {
        _mapName = mapName;
        GameManager.Instance.OnMatchInfoChanged?.Invoke();
    }
}
