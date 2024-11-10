using System.Collections;
using System.Collections.Generic;
using EHTool.UIKit;
using UnityEngine;
using UnityEngine.Serialization;

public class BasePlayer : Character
{
    [SerializeField] protected Pawn[] _pawns;
    [SerializeField] Transform[] _pawnPosition;
    [SerializeField] Color[] _pawnColor;

    IList<Vector3> _emptyPlace;
    public List<IItem> Items => items;

    [SerializeField] protected List<IItem> items;

    [SerializeField] protected int _chance = 0;

    protected int extraDicePoint = 0;

    protected override void DeathEvent()
    {
        foreach (var p in _pawns) {
            p.BackHome();
        }
        base.DeathEvent();
        GameManager.Instance.Playground.PlayerDeath(this);
    }

    IEnumerator WaitAMinute(CallbackMethod callback) {
        yield return new WaitForSeconds(1f);
        callback?.Invoke();
    }

    public void SetInitial(int idx, string name)
    {
        _level = 0;
        _name = name;

        GameManager.Instance.Playground.AddPlayer(this);
        for (int i = 0; i < _pawns.Length; i++)
        {
            _pawns[i].SetOwner(this);

            _pawns[i].SetColor(_pawnColor[idx]);
            _pawns[i].transform.position = _pawnPosition[i].position;
        }

        _emptyPlace = new List<Vector3>();

    }

    public void LeavePawn(Pawn pawn)
    {
        _emptyPlace.Add(pawn.transform.position);
    }

    public void LevelUp(Pawn pawn, int levelUpAmount, bool isPiggyBacked)
    {
        SFXManager.Instance.PlaySFX("PowerUp");
        _level += levelUpAmount;

        pawn.Dispose(_emptyPlace[0]);
        _emptyPlace.RemoveAt(0);

        if (isPiggyBacked) return;

        EndTurn();
    }

    public void BackHomePawn(Pawn pawn)
    {
        pawn.Dispose(_emptyPlace[0]);
        _emptyPlace.RemoveAt(0);

    }

    public virtual void StartTurn()
    {
        _chance = 1;
    }

    public void EndTurn()
    {
        if (_chance == 0)
        {
            GameManager.Instance.Playground.TurnEnd();
            return;
        }
        RollDice();
    }

    public void AddChance()
    {
        _chance++;
    }

    public virtual void RollDice() { }

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

    public virtual void EnterShop(CallbackMethod callback) { }

    public bool BuyItem(IItem item)
    {
        if(item == null) return false;
        if (Money < item.Price) return false;

        Money -= item.Price;
        items.Add(item);
        return true;
    }
    
    public void UseItem(IItem item, CallbackMethod callback = null)
    {
        Debug.Log("USE ITEM!!");
        items.Remove(item);
        item.UseItem(this);
        RollDice(); // !!!!!!! hard coded!!!!!!!!
        callback?.Invoke();
    }

    public void DiscardItem(IItem item)
    {
        items.Remove(item);
    }
    
    public void GetExtraDicePoint(int point)
    {
        extraDicePoint += point;
    }

    public void OpenInventory(CallbackMethod callback)
    {
        UIManager.Instance.OpenGUI<GUIOpenInventory>("Inventory").OpenInventory(this,items);
        callback?.Invoke();
    }

    public void SlainObject()
    {
        PopUpManager.Instance.ShowPopUp(GetName() + " has slain the object!");
        SFXManager.Instance.PlayBGM("3rd");
        _status.AddAttackValue(50);
        _status.AddDefenceValue(50);
    }
}