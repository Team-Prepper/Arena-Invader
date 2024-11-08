using EHTool.UIKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPlayground : GUIFullScreen
{

    [SerializeField] GUIPlayerUnit _playerInfor;

    IList<Character> _players;

    [SerializeField] Button[] _buttons;

    public override void Open()
    {
        base.Open();
        _players = GameManager.Instance.Playground.Players;

        for (int i = 0; i < _buttons.Length; i++) {
            if (i < _players.Count) {
                _buttons[i].gameObject.SetActive(true);
                continue;
            }
            _buttons[i].gameObject.SetActive(false);

        }
    }

    void PressButton(int idx) {

        if (idx >= _buttons.Length) return;

        _playerInfor.SetPlayer(_players[idx]);

    }

}
