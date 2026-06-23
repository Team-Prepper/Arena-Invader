using System;
using Unity.Netcode;
using UnityEngine;
using EasyH.Gaming.TurnBased;

public class UNetCharacterController : NetworkBehaviour, IPlayableCharacter {
    
    [SerializeField] private UNetInventory _inventory;
    private IOpenShop _syncShop;
    private IOpenSelectPawn _syncMovePawn;
    private IOpenDice _syncDice;
    private IOpenInventory _syncInventory;
    private IOpenBattle _syncBattle;


    public IStatus Status { get; private set; }

    public IInventory Inventory { get; private set; }

    public IMemberState TurnState { get; }

    public IPawnOwner PawnOwner { get; private set; }

    private ICharacterController _selector;

    private int _chance = 0;
    private int extraDicePoint = 0;

    void Start()
    {
        _syncShop = gameObject.GetComponent<IOpenShop>();
        _syncMovePawn = gameObject.GetComponent<IOpenSelectPawn>();
        _syncDice = gameObject.GetComponent<IOpenDice>();
        _syncInventory = gameObject.GetComponent<IOpenInventory>();
        _syncBattle = gameObject.GetComponent<IOpenBattle>();

        _syncShop.Initial(this);
        _syncMovePawn.Initial(this);
        _syncDice.Initial(this);
        _syncInventory.Initial(this);
        _syncBattle.Initial(this);

        Status = gameObject.GetComponent<IStatus>();

        Inventory = gameObject.GetComponent<IInventory>();
        Inventory.SetCC(this);

        TurnState.OnTurnEndStateChanged += StartTurn;
        GameManager.Instance.Playground.AddPlayer(this);
    }

    public void SetController(ICharacterController selector) {
        _selector = selector;
    }

    public void Dispose()
    {
        Destroy(gameObject);
    }

    public void StartTurn(bool tmp)
    {
        if (_selector == null) return;

        _chance++;
        _selector.StartTurn(this);
    }

    public void EndTurn()
    {
        if (_selector == null) return;
        if (_chance == 0)
        {
            TurnState.EndTurn();
            return;
        }
        _selector.StartTurn(this);
    }

    public void AddChance()
    {
        _chance++;
    }

    public void GetExtraDicePoint(int point)
    {
        extraDicePoint += point;
    }

    public GUIInventory OpenInventory()
    {
        GUIInventory inventory = _syncInventory.OpenInventory();
        inventory.AddCloseMethod(_selector.RollDice);
        return inventory;
    }

    public void OpenShop(Action callback)
    {
        if (_selector == null) return;
        _syncShop.OpenShop(callback);
        
    }

    public GUIDice OpenRollDice(Action<int> callback)
    {
        _chance--;

        GUIDice dice = _syncDice.OpenDice((value) => {
            callback?.Invoke(value + extraDicePoint);
            extraDicePoint = 0;
        });

        return dice;
    }

    public GUISelectMovePawn OpenSelectMovePawn(int value)
    {
        return _syncMovePawn.OpenSelectMovePawn(value, extraDicePoint);
    }

    public GUIBattle OpenBattle(int targetId, Action callback) {
        if (_selector == null) return null;
        return _syncBattle.OpenBattle(
            TurnState.TeamIdx, targetId, callback);
    }

    public void ShowUseItem(string itemCode)
    {
        _syncInventory.ShowUseItem(itemCode);
    }
}
