using EHTool.UIKit;
using UnityEngine;

public class GUIMatchSetting : GUIFullScreen
{
    [SerializeField] string _path;
    [SerializeField] MatchCharacterDetail[] _details;

    public override void Open()
    {
        base.Open();

        GameManager.Instance.OnMatchInforChanged += SetDetails;
        SetDetails();
    }

    public override void Close()
    {
        base.Close();
        GameManager.Instance.OnMatchInforChanged -= SetDetails;
    }

    public void GenerateMatch()
    {
        GameManager.Instance.Playground.StartMatch();
    }

    void SetDetails()
    {
        IMatchInfor infor = GameManager.Instance.MatchInfor;

        if (infor == null) return;

        for (int i = 0; i < _details.Length; i++) {
            if (i >= infor.PlayerInfors.Count)
            {
                _details[i].gameObject.SetActive(false);
                continue;
            }
            _details[i].gameObject.SetActive(true);
            _details[i].SetDefaultValue(i, infor.PlayerInfors[i].CharacterCode, infor.PlayerInfors[i].Name);
        }
    }

    public void SetPlayerName(int idx, string name)
    {
        GameManager.Instance.MatchInfor.SetPlayerName(idx, name);
    }

    public void SetPlayerCharacter(int idx, string name)
    {
        GameManager.Instance.MatchInfor.SetPlayerCharacter(idx, name);
    }

    public void OpenMatchSetting() {
        GameManager.Instance.MatchInfor.OpenSettingUI();
    }
}
