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
                _buttons[i].SetPlayer(players[i].Status);
                continue;
            }
            _buttons[i].gameObject.SetActive(false);

        }

    }

    private void Update()
    {
        if (GameManager.Instance.Playground.NowPlayer != null)
        {
            _turnInfor.text =
                string.Format(_turnInforFormat,
                    GameManager.Instance.Playground.NowPlayer.Status.Name);

        }

        if (BoardManager.Instance.Map == null) return;

        bool isObject = BoardManager.Instance.Map.GetObject() == null;

        _baronInfoTextGO.gameObject.SetActive(isObject);
        _baronInfor.gameObject.SetActive(!isObject);

        _baronInfoText.text = string.Format(_baronInforFormat,
            TurnManager.Instance.System.TurnSpend);

        if (isObject)
        { 
            _baronInfoText.text =
                string.Format(_baronInforFormat,
                    BoardManager.Instance.Map.GetLeftBaronTurn());
        }
        else
        {
            _baronInfor.SetBaronInfo(
                BoardManager.Instance.Map.GetObject());
        }
       
    }

}
