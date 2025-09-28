using UnityEngine;
using System;
using EasyH.Unity.UI;
using EasyH.Gaming.TurnBased;
using EasyH.Tool.LangKit;
using EasyH.Unity.SoundKit;

public class PlayableCharacter : MonoBehaviour, IPlayableCharacter
{

    private IOpenShop _syncShop;
    private IOpenSelectPawn _syncMovePawn;
    private IOpenDice _syncDice;
    private IOpenInventory _syncInventory;
    private IOpenBattle _syncBattle;

    public IStatus Status { get; private set; }
    public IInventory Inventory { get; private set; }
    public IMemberState TurnState { get; private set; }
    public IPawnOwner PawnOwner { get; private set; }

    private ICharacterController _selector;

    private int _chance = 0;
    private int _extraDicePoint = 0;

    private void Awake()
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
        Status.OnDeathEvent += OnDeath;

        Inventory = gameObject.GetComponent<IInventory>();
        Inventory.SetCC(this);

        TurnState = gameObject.GetComponent<IMemberState>();
        TurnState.OnTurnEndStateChanged += StartTurn;
        TurnState.OnTeamIdxChanged = () =>
        {
            GameManager.Instance.Playground.AddPlayer(this);
        };

        PawnOwner = gameObject.GetComponent<IPawnOwner>();
        PawnOwner.SetCC(this);
    }

    private void OnDeath()
    {
        Status.OnDeathEvent -= OnDeath;
        
        GameManager.Instance.Playground.PlayerDeath(this);
        TurnState.Remove();
    }

    public void Dispose()
    {
        Destroy(gameObject);
    }

    public void SetController(ICharacterController selector)
    {
        _selector = selector;
        Debug.Log(_selector);
    }

    public void StartTurn(bool tmp)
    {
        GUITurnStart turnStartCall = UIManager.
            Instance.OpenGUI<GUITurnStart>("TurnStart");

        SoundManager.Instance.PlaySFX("TurnStart");

        turnStartCall.SetMessage(string.Format(
            LangManager.Instance.GetStringByKey("msg_XTurn"),
                Status.Name));

        turnStartCall.SetWaitForCallback(
            () =>
            {
                if (_selector != null)
                {
                    _chance++;
                    _selector.StartTurn(this);
                }
                turnStartCall.Close();
            });
    }

    public void EndTurn()
    {
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
        _extraDicePoint += point;
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

        GUIDice dice = _syncDice.OpenDice((value) =>
        {
            callback?.Invoke(value + _extraDicePoint);
            _extraDicePoint = 0;
        });

        return dice;
    }

    public GUISelectMovePawn OpenSelectMovePawn(int value)
    {

        return _syncMovePawn.OpenSelectMovePawn(
            value, _extraDicePoint);
    }

    public GUIBattle OpenBattle(int targetId, Action callback)
    {

        if (_selector == null) return null;
        return _syncBattle.OpenBattle(TurnState.TeamIdx, targetId, callback);

    }

    public void ShowUseItem(string itemCode)
    {
        _syncInventory.ShowUseItem(itemCode);
    }

}