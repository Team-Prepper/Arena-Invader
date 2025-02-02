using EHTool.UIKit;
using UnityEngine;
using System;

public class LocalCharacterController : MonoBehaviour, ICharacterController {

    public BasePlayer Target { get; private set; }
    ICharacterActionSelector _selector;

    [SerializeField] private int _chance = 0;
    private int extraDicePoint = 0;

    string _name;

    public void SetMatch(ICharacterActionSelector selector)
    {
        _selector = selector;
    }

    public void SetTargetCharacter(string name, string characterCode, int idx)
    {
        GameManager.Instance.Playground.AddPlayer(this);

        Target = CharacterManager.Instance.SpawnPlayer(characterCode);
        Target.transform.SetParent(transform);
        Target.transform.localPosition = Vector3.zero;
        Target.SetInitial(this, name, idx);

        _name = name;

    }

    public void StartTurn()
    {
        _chance++;

        _selector.StartTurn(this);
    }

    public void AbilityChange(string abilityType, string amount)
    {

    }

    public void MovePawn(int pawnId, int amount)
    {
        Target._pawns[pawnId].Move(amount);
    }

    public void EndTurn()
    {
        if (_chance == 0)
        {
            GameManager.Instance.Playground.TurnEnd();
            return;
        }
        _selector.StartTurn(this);
    }

    public void AddChance() {
        _chance++;
    }

    public void GetExtraDicePoint(int point)
    {
        extraDicePoint += point;
    }
    public GUIOpenInventory OpenInventory(Action<int> value)
    {
        GUIOpenInventory inventory = UIManager.Instance.OpenGUI<GUIOpenInventory>("Inventory");
        return inventory;
    }

    public void EnterShop(Action callback) {
        GUIShop shop = UIManager.Instance.OpenGUI<GUIShop>("Shop");

        shop.EnterShop(Target, () =>
        {
            callback?.Invoke();
            shop.Close();
        });

        _selector.SelectItem(shop, (value) => { });
    }

    public GUIDice RollDice(Action<int> callback)
    {
        _chance--;

        GUIDice gui = UIManager.Instance.OpenGUI<GUIDice>
            (GameManager.Instance.MatchInfor.MatchDice);

        gui.SetCallback((value) => {
            callback?.Invoke(value + extraDicePoint);
            extraDicePoint = 0;
            gui.Close();
        });
        
        return gui;
    }

    public GUISelectMovePawn SelectMovePawn(int value) {

        GUISelectMovePawn movePawn =
            UIManager.Instance.OpenGUI<GUISelectMovePawn>("SelectMovePawn");

        Target.OnPawnChoose();

        movePawn.SetPlayer(this, value, () => {
            Target.OffPawnChoose();
            movePawn.Close();
        });

        return movePawn;
    }

}