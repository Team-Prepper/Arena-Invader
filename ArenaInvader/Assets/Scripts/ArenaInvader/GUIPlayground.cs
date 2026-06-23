using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyH.Unity.UI;
using EasyH.Gaming.TurnBased;

public class GUIPlayground : GUIFullScreen
{

    [SerializeField] GUIUnitPlayerButton[] _buttons;
    [SerializeField] GUIUnitPlayer _playerInfor;
    [SerializeField] Text _turnInfor;
    [SerializeField] string _turnInforFormat = "{0}'s Turn";

    [Header("Baron Infor")]
    [SerializeField] GUIBaronInfo _baronInfor;
    [SerializeField] GameObject _baronInfoTextGO;
    [SerializeField] Text _baronInfoText;
    [SerializeField] string _baronInforFormat = "{0} Turn left";

    [Header("generator")]
    [SerializeField] MatchGenerator _generator;

    private string _lastTurnText;
    private bool? _lastIsObject;
    private int _lastBaronTurn = int.MinValue;
    private IStatus _lastBaronStatus;

    public override void Open()
    {
        base.Open();

        if (_generator)
        {
            _generator.GenerateMap();
            _generator.Generate();
            Generate();
        }

    }

    public void Generate()
    {
        IList<IPlayableCharacter> players
            = GameManager.Instance.Playground.Players;

        for (int i = 0; i < _buttons.Length; i++)
        {
            if (i < players.Count)
            {
                _buttons[i].gameObject.SetActive(true);
                _buttons[i].SetPlayerInfoPanel(_playerInfor);
                _buttons[i].SetPlayer(players[i]);
                continue;
            }
            _buttons[i].gameObject.SetActive(false);

        }

    }

    private void Update()
    {
        UpdateTurnInfo();
        UpdateBaronInfo();
    }

    private void UpdateTurnInfo()
    {
        IPlayableCharacter nowPlayer = GameManager.Instance.Playground.NowPlayer;
        if (nowPlayer == null || nowPlayer.Status == null)
        {
            return;
        }

        string turnText = string.Format(_turnInforFormat, nowPlayer.Status.Name);
        if (_lastTurnText == turnText)
        {
            return;
        }

        _lastTurnText = turnText;
        _turnInfor.text = turnText;
    }

    private void UpdateBaronInfo()
    {
        if (BoardManager.Instance.Map == null) return;

        IStatus baronStatus = BoardManager.Instance.Map.GetObject();
        bool isObject = baronStatus == null;

        if (_lastIsObject != isObject)
        {
            _baronInfoTextGO.gameObject.SetActive(isObject);
            _baronInfor.gameObject.SetActive(!isObject);
            _lastIsObject = isObject;
        }

        if (isObject)
        {
            int leftTurn = BoardManager.Instance.Map.GetLeftBaronTurn();
            if (_lastBaronTurn == leftTurn)
            {
                return;
            }

            _lastBaronTurn = leftTurn;
            _baronInfoText.text = string.Format(_baronInforFormat, leftTurn);
        }
        else
        {
            if (_lastBaronStatus == baronStatus)
            {
                return;
            }

            _lastBaronStatus = baronStatus;
            _baronInfor.SetBaronInfo(baronStatus);
        }
    }

}
