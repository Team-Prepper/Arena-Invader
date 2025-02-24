using EHTool.UIKit;
using UnityEngine;
using System;

public class LocalCharacterController : MonoBehaviour, ICharacterController {

    public int PlayerId { get; private set; }

    [SerializeField] private LocalStatus _status;
    public IStatus Status => _status;

    public BasePlayer Target { get; private set; }
    private ICharacterActionSelector _selector;

    [SerializeField] private int _chance = 0;
    private int extraDicePoint = 0;

    public void SetMatch(ICharacterActionSelector selector)
    {
        _selector = selector;
    }

    public void SetTargetCharacter(string name, string characterCode, int idx)
    {
        PlayerId = idx;

        GameManager.Instance.Playground.AddPlayer(this);

        _status = gameObject.GetComponent<LocalStatus>();

        Status.Name = name;
        Status.CharacterCode = characterCode;
        Status.SetCC(this);

        Target = CharacterManager.Instance.SpawnPlayer(characterCode);
        Target.transform.SetParent(transform);
        Target.transform.localPosition = Vector3.zero;
        Target.SetInitial(this, idx);

    }

    public void StartTurn()
    {
        _chance++;

        _selector.StartTurn(this);
    }

    public void MovePawn(int pawnId, int amount)
    {
        Target.Pawns[pawnId].Move(amount);
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

    public GUIOpenInventory OpenInventory()
    {
        GUIOpenInventory inventory = UIManager.Instance.OpenGUI<GUIOpenInventory>("Inventory");
        inventory.SetTarget(
            ItemManager.Instance.ItemListToInt(Status.Items),
            Status.Items.Count, this);
        return inventory;
    }

    public void OpenShop(Action callback) {
        GUIShop shop = UIManager.Instance.OpenGUI<GUIShop>("Shop");

        shop.SetBuyer(this);
        shop.SetItems(ItemManager.Instance.RandomItemListByInt(shop.Size));

        shop.SetCloseMethod(() =>
        {
            callback?.Invoke();
        });

        _selector.Shop(shop);
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

        movePawn.SetPlayer(this, value);
        movePawn.SetCloseMethod(() =>
        {
            Target.OffPawnChoose();
        });

        return movePawn;
    }

}