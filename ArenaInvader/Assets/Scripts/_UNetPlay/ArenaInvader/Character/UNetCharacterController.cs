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

    public IMemberState TurnState { get; private set; }

    public IPawnOwner PawnOwner { get; private set; }

    private UNetCharacterTurnState _turnState;

    private void Awake()
    {
        _syncShop = RequireComponent<IOpenShop>();
        _syncMovePawn = RequireComponent<IOpenSelectPawn>();
        _syncDice = RequireComponent<IOpenDice>();
        _syncInventory = RequireComponent<IOpenInventory>();
        _syncBattle = RequireComponent<IOpenBattle>();

        Status = RequireComponent<IStatus>();
        Status.OnDeathEvent += OnDeath;

        Inventory = RequireComponent<IInventory>();
        Inventory.SetCC(this);

        _syncShop.Initial(this);
        _syncMovePawn.Initial(this);
        _syncDice.Initial(this);
        _syncInventory.Initial(this);
        _syncBattle.Initial(this);

        TurnState = RequireComponent<IMemberState>();
        TurnState.OnTurnEndStateChanged += StartTurn;
        TurnState.OnTeamIdxChanged = () =>
        {
            GameManager.Instance.Playground.AddPlayer(this);
        };

        PawnOwner = RequireComponent<IPawnOwner>();
        PawnOwner.SetCC(this);

        _turnState = GetComponent<UNetCharacterTurnState>();
        if (_turnState == null)
        {
            _turnState = gameObject.AddComponent<UNetCharacterTurnState>();
        }
    }

    private void Start()
    {
        if (TurnState.TeamIdx >= 0)
        {
            GameManager.Instance.Playground.AddPlayer(this);
        }
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    private T RequireComponent<T>() where T : class
    {
        T component = GetComponent(typeof(T)) as T;
        if (component == null)
        {
            throw new MissingComponentException(
                $"{name} requires component {typeof(T).Name}.");
        }

        return component;
    }

    private void UnsubscribeEvents()
    {
        if (Status != null)
        {
            Status.OnDeathEvent -= OnDeath;
        }

        if (TurnState != null)
        {
            TurnState.OnTurnEndStateChanged -= StartTurn;
        }
    }

    private void OnDeath()
    {
        Status.OnDeathEvent -= OnDeath;
        GameManager.Instance?.Playground?.PlayerDeath(this);
        TurnState.Remove();
    }

    public void SetController(ICharacterController selector) {
        _turnState.SetController(selector);
    }

    public void Dispose()
    {
        Destroy(gameObject);
    }

    public void StartTurn(bool tmp)
    {
        _turnState.BeginTurn(this);
    }

    public void EndTurn()
    {
        _turnState.EndTurn(TurnState, this);
    }

    public void AddChance()
    {
        _turnState.AddChance();
    }

    public void GetExtraDicePoint(int point)
    {
        _turnState.AddExtraDicePoint(point);
    }

    public GUIInventory OpenInventory()
    {
        GUIInventory inventory = _syncInventory.OpenInventory();
        if (_turnState.HasController())
        {
            inventory.AddCloseMethod(_turnState.StartRollDice);
        }

        return inventory;
    }

    public void OpenShop(Action callback)
    {
        if (!_turnState.HasController()) return;
        GUIShop shop = _syncShop.OpenShop(callback);
        _turnState.OpenShop(shop);
    }

    public GUIDice OpenRollDice(Action<int> callback)
    {
        _turnState.SpendChance();

        GUIDice dice = _syncDice.OpenDice((value) => {
            callback?.Invoke(value + _turnState.ConsumeExtraDicePoint());
        });

        return dice;
    }

    public GUISelectMovePawn OpenSelectMovePawn(int value)
    {
        return _syncMovePawn.OpenSelectMovePawn(
            value, _turnState.GetExtraDicePoint());
    }

    public GUIBattle OpenBattle(int targetId, Action callback) {
        if (!_turnState.HasController()) return null;
        return _syncBattle.OpenBattle(
            TurnState.TeamIdx, targetId, callback);
    }

    public void ShowUseItem(string itemCode)
    {
        _syncInventory.ShowUseItem(itemCode);
    }
}
