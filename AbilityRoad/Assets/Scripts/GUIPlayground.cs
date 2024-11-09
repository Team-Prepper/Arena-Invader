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

    [SerializeField] MatchGenerator _generator;

    public override void Open()
    {
        base.Open();
        GenerateWorld();
    }

    void GenerateWorld()
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
    }

    public void PressButton(int idx) {

        if (idx >= _buttons.Length) return;

        _playerInfor.SetPlayer(_players[idx]);

    }

}
