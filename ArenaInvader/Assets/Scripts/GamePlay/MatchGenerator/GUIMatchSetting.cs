using EHTool.UIKit;
using UnityEngine;

public class GUIMatchSetting : GUIFullScreen
{
    [SerializeField] private MatchCharacterDetail[] _details;
    [SerializeField] private GameObject _startBtn;
    [SerializeField] private GameObject _readyBtn;
    [SerializeField] private GameObject _settingBtn;

    public override void Open()
    {
        base.Open();

        if (GameManager.Instance.MatchInfor == null)
        {
            GameManager.Instance.OnMatchInforChanged += SetUI;
        }
        else
        {
            SetUI();
            SetDetails();
        }

        GameManager.Instance.OnMatchInforChanged += SetDetails;
    }

    public override void Close()
    {
        GameManager.Instance.OnMatchInforChanged -= SetDetails;
        base.Close();
    }


    void SetDetails()
    {
        IMatchInfor infor = GameManager.Instance.MatchInfor;

        bool isMaster = infor.EditableIdx.Contains(0);

        _readyBtn.SetActive(!isMaster);
        _startBtn.SetActive(isMaster);
        _settingBtn.SetActive(isMaster);

        for (int i = 0; i < _details.Length; i++) {
            _details[i].SetDefaultValue(infor.PlayerInfors,
                i, infor.EditableIdx.Contains(i));
        }
    }

    public void SetPlayerName(int idx, string name)
    {
        if (!GameManager.Instance.MatchInfor.
            EditableIdx.Contains(idx)) return;

        GameManager.Instance.MatchInfor.
            SetPlayerName(idx, name);
    }

    public void SetPlayerCharacter(int idx, string name)
    {
        if (!GameManager.Instance.MatchInfor.
            EditableIdx.Contains(idx)) return;

        GameManager.Instance.MatchInfor.
            SetPlayerCharacter(idx, name);
    }

    public void OpenMatchSetting() {
        if (GameManager.Instance.MatchInfor == null) return;
        GameManager.Instance.MatchInfor.OpenSettingUI();
    }

    public void SetUI()
    {
        GameManager.Instance.OnMatchInforChanged -= SetUI;
        GameManager.Instance.MatchInfor.SetMatchSettingUI(this);

    }

    public void Dispose()
    {
        GameManager.Instance.MatchInfor.Dispose();

    }

    public void GenerateMatch()
    {
        GameManager.Instance.MatchInfor.StartMatch();
    }

}
