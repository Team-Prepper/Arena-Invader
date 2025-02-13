using System.Collections.Generic;
using EHTool.UIKit;
using UnityEngine;
using System;

public class BasePlayer : Character
{
    [SerializeField] public Pawn[] _pawns;
    [SerializeField] Transform[] _pawnPosition;
    [SerializeField] Color[] _pawnColor;

    ICharacterController _cc;

    IList<Vector3> _emptyPlace;

    public List<ItemData> Items => items;

    [SerializeField] public List<ItemData> items;

    public void SetInitial(ICharacterController cc, string name, int idx)
    {
        _cc = cc;
        _level = 0;
        _name = name;

        for (int i = 0; i < _pawns.Length; i++)
        {
            _pawns[i].SetOwner(this, i);

            _pawns[i].SetColor(_pawnColor[idx]);
            _pawns[i].transform.position = _pawnPosition[i].position;
        }

        _emptyPlace = new List<Vector3>();

    }

    protected override void DeathEvent()
    {
        foreach (var p in _pawns) {
            p.BackHome();
        }
        base.DeathEvent();
        GameManager.Instance.Playground.PlayerDeath(_cc);
    }

    public void LeavePawn(int id)
    {
        _emptyPlace.Add(_pawns[id].transform.position);
    }

    public void LevelUp(int id, int levelUpAmount, bool isPiggyBacked)
    {
        SFXManager.Instance.PlaySFX("PowerUp");
        _level += levelUpAmount;

        _pawns[id].Dispose(_emptyPlace[0]);
        _emptyPlace.RemoveAt(0);

        if (isPiggyBacked) return;

        EndTurn();
    }

    public void BackHomePawn(int id)
    {
        _pawns[id].Dispose(_emptyPlace[0]);
        _emptyPlace.RemoveAt(0);

    }

    public void EndTurn()
    {
        _cc.EndTurn();
    }

    public void AddChance()
    {
        _cc.AddChance();
    }

    public void OnPawnChoose()
    {
        for (int i = 0; i < _pawns.Length; i++)
        {
            _pawns[i].EnterTurn();
        }

    }
    public void OffPawnChoose()
    {
        for (int i = 0; i < _pawns.Length; i++)
        {
            _pawns[i].ExitTurn();
        }
    }

    public void EnterShop(Action callback) {
        _cc.OpenShop(callback);
    }

    public bool BuyItem(ItemData item)
    {
        if(item == null) return false;
        if (Money < item.Price) return false;

        Money -= item.Price;
        items.Add(item);
        return true;
    }
    
    public void UseItem(ItemData item, Action callback = null)
    {
        Debug.Log("USE ITEM!!");
        items.Remove(item);
        item.Item.UseItem(_cc);
        //RollDice(); // !!!!!!! hard coded!!!!!!!!
        callback?.Invoke();
    }

    public void DiscardItem(ItemData item)
    {
        items.Remove(item);
    }

    public void SlainObject()
    {
        PopUpManager.Instance.ShowPopUp(GetName() + " has slain the object!");
        SFXManager.Instance.PlayBGM("3rd");
        _status.AddAttackValue(50);
        _status.AddDefenceValue(50);
    }

}