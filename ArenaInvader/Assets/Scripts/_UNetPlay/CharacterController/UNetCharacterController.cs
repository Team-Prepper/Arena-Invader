using System;
using Unity.Netcode;
using UnityEngine;

public class UNetCharacterController : NetworkBehaviour, ICharacterController {
    
    [SerializeField] private UNetStatus _status;
    [SerializeField] private UNetInventory _inventory;

    [SerializeField] private UNetSyncShop _syncShop;
    [SerializeField] private UNetSyncMovePawn _syncMovePawn;
    [SerializeField] private UNetSyncDice _syncDice;
    [SerializeField] private UNetSyncInventory _syncInventory;
    [SerializeField] private UNetSyncBattle _syncBattle;

    public int PlayerId { get; set; }

    public BasePlayer Target { get; private set; }
    
    public IStatus Status => _status;

    public IInventory Inventory => _inventory;

    private ICharacterActionSelector _selector;

    private int _chance = 0;
    private int extraDicePoint = 0;

    void Start()
    {
        _syncShop.Initial(this);
        _syncMovePawn.Initial(this);
        _syncDice.Initial(this);
        _syncInventory.Initial(this);
        _syncBattle.Initial(this);
    }

    public void SetMatch(ICharacterActionSelector selector) {
        _selector = selector;
    }

    public void SetTargetCharacter(string name, string characterCode, int idx)
    {
        _status = gameObject.GetComponent<UNetStatus>();

        Status.Name = name;
        Status.CharacterCode = characterCode;
        
        Inventory.SetCC(this);
        
        SetTargetCharacterClientRpc(name, characterCode, idx);
    }

    [ClientRpc]
    void SetTargetCharacterClientRpc(string name, string characterCode, int idx) {

        PlayerId = idx;
        GameManager.Instance.Playground.AddPlayer(this);
        
        _status = gameObject.GetComponent<UNetStatus>();
        Status.Name = name;
        Status.CharacterCode = characterCode;

        Target = CharacterManager.Instance.SpawnPlayer(characterCode).GetComponent<BasePlayer>();
        Target.transform.SetParent(transform);
        Target.transform.localPosition = Vector3.zero;

        Target.SetInitial(this, idx);

    }

    public void StartTurn()
    {
        if (_selector == null) return;

        _chance++;
        _selector.StartTurn(this);
    }

    public void MovePawn(int pawnId, int amount)
    {
        Target.OffPawnChoose();
        MovePawnServerRpc(pawnId, amount);
    }

    [ServerRpc(RequireOwnership = false)]
    public void MovePawnServerRpc(int pawnId, int amount)
        => MovePawnClientRpc(pawnId, amount);

    [ClientRpc]
    public void MovePawnClientRpc(int pawnId, int amount)
    {
        Target.Pawns[pawnId].Move(amount);
    }

    public void EndTurn()
    {
        if (_selector == null) return;
        if (_chance == 0)
        {
            GameManager.Instance.Playground.TurnEnd();
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
        return _syncBattle.OpenBattle(PlayerId, targetId, callback);
    }
    public void ShowUseItem(string itemCode) {
        ShowUseItemServerRpc(itemCode);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ShowUseItemServerRpc(string itemCode)
        => ShowUseItemClientRpc(itemCode);

    [ClientRpc]
    public void ShowUseItemClientRpc(string itemCode)
    {
        EHTool.UIKit.UIManager.Instance.
            OpenGUI<GUIUseItemShow>("UseItemShow").SetUseItem(itemCode);
    }

}