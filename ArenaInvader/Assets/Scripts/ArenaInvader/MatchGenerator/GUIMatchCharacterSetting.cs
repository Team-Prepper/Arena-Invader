using UnityEngine;
using EasyH.Unity.UI;

public class GUIMatchCharacterSetting : GUIFullScreen
{
    [SerializeField] private MatchCharacterDetail[] _details;
    [SerializeField] private GameObject _startBtn;
    [SerializeField] private GameObject _readyBtn;
    [SerializeField] private GameObject _settingBtn;


    [SerializeField] private GameObject _onLoading;
    [SerializeField] private GameObject _onLoadingEnd;

    public override void Open()
    {
        base.Open();

        if (GameManager.Instance.MatchInfo == null)
        {
            _onLoading.SetActive(true);
            _onLoadingEnd.SetActive(false);
            GameManager.Instance.OnMatchInfoChanged += SetUI;
        }
        else
        {
            SetUI();
            SetDetails();
        }

        GameManager.Instance.OnMatchInfoChanged += SetDetails;
    }

    public override void Close()
    {
        GameManager.Instance.OnMatchInfoChanged -= SetDetails;
        base.Close();
    }


    void SetDetails()
    {
        IMatchInfo infor = GameManager.Instance.MatchInfo;

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
        if (!GameManager.Instance.MatchInfo.
            EditableIdx.Contains(idx)) return;

        GameManager.Instance.MatchInfo.
            SetPlayerName(idx, name);
    }

    public void SetPlayerCharacter(int idx, string name)
    {
        if (!GameManager.Instance.MatchInfo.
            EditableIdx.Contains(idx)) return;

        GameManager.Instance.MatchInfo.
            SetPlayerCharacter(idx, name);
    }

    public void SetPlayerIsAI(int idx, bool isAI)
    {
        if (!GameManager.Instance.MatchInfo.
            EditableIdx.Contains(idx)) return;

        GameManager.Instance.MatchInfo.
            SetPlayerIsAI(idx, isAI);
    }

    public void OpenMatchSetting() {
        if (GameManager.Instance.MatchInfo == null) return;
        GameManager.Instance.MatchInfo.OpenSettingUI();
    }

    public void SetUI()
    {
        GameManager.Instance.OnMatchInfoChanged -= SetUI;

        _onLoading.SetActive(false);
        _onLoadingEnd.SetActive(true);
        GameManager.Instance.MatchInfo.SetMatchSettingUI(this);

    }

    public void Dispose()
    {
        GameManager.Instance.MatchInfo.Dispose();

    }

    public void GenerateMatch()
    {
        GameManager.Instance.MatchInfo.StartMatch();
    }

}
