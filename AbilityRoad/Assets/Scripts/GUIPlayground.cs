using EHTool.UIKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPlayground : GUIFullScreen
{
    IList<BasePlayer> _players;

    [SerializeField] GUIPlayerButtonUnit[] _buttons;
    [SerializeField] GUIPlayerUnit _playerInfor;
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
        if (_generator) {
            Generate();
        }
        // 
    }

    public void GenerateMatch(MatchInfor infor)
    {
        _generator = GameObject.FindWithTag("MatchGenerator").GetComponent<MatchGenerator>();
        GameManager.Instance.Playground.SetMatchDice(infor.MatchDice);
        _generator.SetMatchInfor(infor);
        Generate();
    }

    void Generate()
    {
        _generator.Generate();

        _players = GameManager.Instance.Playground.Players;

        for (int i = 0; i < _buttons.Length; i++)
        {
            if (i < _players.Count)
            {
                _buttons[i].gameObject.SetActive(true);
                _buttons[i].SetPlayer(_players[i]);
                continue;
            }
            _buttons[i].gameObject.SetActive(false);

        }

        PressButton(0);

    }

    private void Update()
    {
        _turnInfor.text =
            string.Format(_turnInforFormat, GameManager.Instance.Playground.NowPlayer.GetName());

        if (GameManager.Instance.Playground.Map.GetObject() == null)
        { 
            _baronInfoTextGO.gameObject.SetActive(true);
            _baronInfor.gameObject.SetActive(false);
            _baronInfoText.text =
                string.Format(_baronInforFormat, GameManager.Instance.Playground.Map.GetLeftBaronTurn());
        }
        else
        {
            _baronInfoTextGO.gameObject.SetActive(false);
            _baronInfor.gameObject.SetActive(true);
            _baronInfor.SetBaronInfo(GameManager.Instance.Playground.Map.GetObject());
        }
       
    }

    public void PressButton(int idx) {

        if (idx >= _buttons.Length) return;

    }

}
