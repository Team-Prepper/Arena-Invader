using UnityEngine;
using UnityEngine.UI;
using EasyH.Unity.UI;


[System.Serializable]
struct StringTransform
{
    public string Key;
    public RectTransform transform;

}

public class GUIMatchSetting : GUIPopUp
{

    [System.Serializable]
    public class Options
    {
        public string key;
        public string value;

    }

    [SerializeField] private Text _playerCnt;
    [SerializeField] private EHDropdownWrapper _mapDropdown;
    [SerializeField] private EHDropdownWrapper _diceDropdown;

    [SerializeField] private Options[] _mapOptions;
    [SerializeField] private Options[] _diceOptions;

    private string[] OptionArrayToKeyArray(Options[] options)
    {
        string[] retval = new string[options.Length];
        for (int i = 0; i < options.Length; i++)
        {
            retval[i] = options[i].key;
        }
        return retval;
    }

    public override void Open()
    {
        GameManager.Instance.OnMatchInfoChanged += OnMatchInfoChanged;

        _mapDropdown.SetDropdownOption(OptionArrayToKeyArray(_mapOptions));
        _mapDropdown.onValueChanged.AddListener(SetMap);

        _diceDropdown.SetDropdownOption(OptionArrayToKeyArray(_diceOptions));
        _diceDropdown.onValueChanged.AddListener(SetDice);

        OnMatchInfoChanged();

        base.Open();
    }

    public override void Close()
    {
        GameManager.Instance.OnMatchInfoChanged -= OnMatchInfoChanged;
        base.Close();

    }

    public void SetPlayerCnt(int amount)
    {
        int cnt = Mathf.Clamp(amount + GameManager.Instance.MatchInfo.PlayerInfors.Count, 2, 4);
        GameManager.Instance.MatchInfo.SetPlayerCnt(cnt);

    }

    public void SetDice(int idx)
    {
        GameManager.Instance.MatchInfo.SetDice(_diceOptions[idx].value);
    }

    public void SetMap(int idx)
    {
        GameManager.Instance.MatchInfo.SetMap(_mapOptions[idx].value);
    }

    private int FindIdxInArray(Options[] options, string value)
    {
        for (int i = 0; i < options.Length; i++)
        {
            if (options[i].value.Equals(value)) return i;
        }
        return -1;
    }

    void OnMatchInfoChanged()
    {

        _diceDropdown.value =
            FindIdxInArray(_diceOptions, GameManager.Instance.MatchInfo.MatchDice);
        _mapDropdown.value =
            FindIdxInArray(_mapOptions, GameManager.Instance.MatchInfo.MapName);

        if (_playerCnt == null) return;

        _playerCnt.text = GameManager.Instance.MatchInfo.PlayerInfors.Count.ToString();

    }

}
