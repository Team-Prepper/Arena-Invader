using EHTool.UIKit;
using UnityEngine;


[System.Serializable]
struct StringTransform {
    public string Key;
    public RectTransform transform;


}

public class GUILocalMatchSetting : GUIPopUp {

    [SerializeField] StringTransform[] _playerCntST;
    [SerializeField] StringTransform[] _mapST;
    [SerializeField] StringTransform[] _diceST;

    [SerializeField] RectTransform _playerCntCursor;
    [SerializeField] RectTransform _mapCursor;
    [SerializeField] RectTransform _diceCursor;

    public override void Open()
    {
        GameManager.Instance.OnMatchInforChanged += OnMatchInforChanged;
        OnMatchInforChanged();
        base.Open();
    }

    public override void Close()
    {
        GameManager.Instance.OnMatchInforChanged -= OnMatchInforChanged;
        base.Close();

    }

    public void SetPlayerCnt(int cnt)
    {
        GameManager.Instance.MatchInfor.SetPlayerCnt(cnt);
    }

    public void SetDice(string diceCode)
    {
        GameManager.Instance.MatchInfor.SetDice(diceCode);
    }

    public void SetMap(string mapName)
    {
        GameManager.Instance.MatchInfor.SetMap(mapName);
    }

    Transform FindIdx(StringTransform[] target, string key) {

        foreach (var st in target) {
            if (st.Key.Equals(key)) {
                return st.transform;
            }
        }

        return target[0].transform;
    }

    void CursorSet(Transform cursor, StringTransform[] target, string key) {

        cursor.SetParent(FindIdx(target, key));
        cursor.localPosition = Vector3.zero;

    }

    void OnMatchInforChanged() {

        CursorSet(_diceCursor, _diceST, GameManager.Instance.MatchInfor.MatchDice);

        CursorSet(_mapCursor, _mapST, GameManager.Instance.MatchInfor.MapName);

        if (_playerCntCursor == null) return;

        CursorSet(_playerCntCursor, _playerCntST,
            GameManager.Instance.MatchInfor.PlayerInfors.Count.ToString());

    }

}
