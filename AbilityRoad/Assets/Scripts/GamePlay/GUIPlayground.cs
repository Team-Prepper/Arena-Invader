using EHTool;
using EHTool.UIKit;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GUIPlayground : GUIFullScreen
{
    IList<ICharacterController> _players;

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
            _generator.Generate();
            Generate();
        }

    }

    public void Generate()
    {
        _players = GameManager.Instance.Playground.Players;

        for (int i = 0; i < _buttons.Length; i++)
        {
            if (i < _players.Count)
            {
                _buttons[i].gameObject.SetActive(true);
                _buttons[i].SetPlayer(_players[i].Target);
                continue;
            }
            _buttons[i].gameObject.SetActive(false);

        }

        PressButton(0);

    }

    private void Update()
    {
        if (GameManager.Instance.Playground.NowPlayer != null)
        {
            _turnInfor.text =
                string.Format(_turnInforFormat, GameManager.Instance.Playground.NowPlayer.Target.GetName());

        }

        if (GameManager.Instance.Playground.Map == null) return;

        bool isObject = GameManager.Instance.Playground.Map.GetObject() == null;

        _baronInfoTextGO.gameObject.SetActive(isObject);
        _baronInfor.gameObject.SetActive(!isObject);

        if (isObject)
        { 
            _baronInfoText.text =
                string.Format(_baronInforFormat, GameManager.Instance.Playground.Map.GetLeftBaronTurn());
        }
        else
        {
            _baronInfor.SetBaronInfo(GameManager.Instance.Playground.Map.GetObject());
        }
       
    }

    public void PressButton(int idx) {

        if (idx >= _buttons.Length) return;

    }

}
