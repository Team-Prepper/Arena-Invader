using EHTool.UIKit;
using UnityEngine;

public class GUIMatchSetting : GUIFullScreen
{
    [SerializeField] string _path;
    [SerializeField] MatchCharacterDetail[] _details;

    [SerializeField] Transform _playerCntCursor;
    [SerializeField] Transform _mapCursor;
    [SerializeField] Transform _diceCursor;

    public void PlayerCntCursorPos(Transform target) {
        _playerCntCursor.position = target.position;
    }
    public void MapCursorPos(Transform target)
    {
        _mapCursor.position = target.position;

    }
    public void DiceCursorPos(Transform target)
    {
        _diceCursor.position = target.position;

    }

    public override void Open()
    {
        base.Open();
        SetDetails();
    }

    public void GenerateMatch()
    {
        UIManager.Instance.OpenGUI<GUIPlayground>("Playground")
            .GenerateMatch(GameManager.Instance.Playground.MatchInfor);
    }

    public void SetPlayerCnt(int cnt) {

        GameManager.Instance.Playground.MatchInfor.SetPlayerCnt(cnt);
        SetDetails();
    }

    void SetDetails()
    {
        MatchInfor _infor = GameManager.Instance.Playground.MatchInfor;

        for (int i = 0; i < _details.Length; i++) {
            if (i >= _infor.PlayerInfors.Length)
            {
                _details[i].gameObject.SetActive(false);
                continue;
            }
            _details[i].gameObject.SetActive(true);
            _details[i].SetDefaultValue(i, _infor.PlayerInfors[i].CharacterCode, _infor.PlayerInfors[i].Name);
        }
    }

    public void SetDice(string diceCode)
    {
        GameManager.Instance.Playground.MatchInfor.SetDice(diceCode);
    }

    public void SetPlayerName(int idx, string name)
    {
        GameManager.Instance.Playground.MatchInfor.SetPlayerName(idx, name);
    }

    public void SetPlayerCharacter(int idx, string name)
    {
        GameManager.Instance.Playground.MatchInfor.SetPlayerCharacter(idx, name);
    }

    public void SetMap(string mapName)
    {
        GameManager.Instance.Playground.MatchInfor.SetMap(mapName);
    }
}
