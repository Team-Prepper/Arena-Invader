using System;
using Unity.Netcode;
using UnityEngine;

public class UNetCharacterController : NetworkBehaviour, ICharacterController {
    
    public BasePlayer Target { get; private set; }
    ICharacterActionSelector _selector;

    [SerializeField] private int _chance = 0;
    private int extraDicePoint = 0;

    [SerializeField] UNetSyncShop _syncShop;
    [SerializeField] UNetSyncMovePawn _syncMovePawn;
    [SerializeField] UNetSyncDice _syncDice;
    [SerializeField] UNetSyncInventory _syncInventory;

    void Start()
    {
        _syncShop.Initial(this);
        _syncMovePawn.Initial(this);
        _syncDice.Initial(this);
        _syncInventory.Initial(this);
    }

    public void SetMatch(ICharacterActionSelector selector) {
        _selector = selector;
    }

    public void SetTargetCharacter(string name, string characterCode, int idx)
    {
        SetTargetCharacterClientRpc(characterCode, name, idx);
    }

    [ClientRpc]
    void SetTargetCharacterClientRpc(string name, string characterCode, int idx) {

        Target = CharacterManager.Instance.SpawnPlayer(characterCode);
        Target.transform.SetParent(transform);
        Target.transform.localPosition = Vector3.zero;
        GameManager.Instance.Playground.AddPlayer(this);

        Target.SetInitial(this, name, idx);
    }

    public void StartTurn()
    {
        if (_selector == null) return;

        _chance++;
        _selector.StartTurn(this);
    }

    public void AbilityChange(string abilityType, string amount)
    {

    }

    public void MovePawn(int pawnId, int amount)
    {
        MovePawnServerRpc(pawnId, amount);
    }

    [ServerRpc(RequireOwnership = false)]
    public void MovePawnServerRpc(int pawnId, int amount) => MovePawnClientRpc(pawnId, amount);

    [ClientRpc]
    public void MovePawnClientRpc(int pawnId, int amount)
    {
        Target._pawns[pawnId].OffFocus();
        Target._pawns[pawnId].Move(amount);
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

    public GUIOpenInventory OpenInventory()
    {
        GUIOpenInventory inventory = _syncInventory.OpenInventory();
        return inventory;
    }

    public void OpenShop(Action callback)
    {
        if (_selector == null) return;

        GUIShop shop = _syncShop.OpenShop(callback);
        _selector.Shop(shop);
        
    }

    public GUIDice RollDice(Action<int> callback)
    {
        _chance--;

        GUIDice dice = _syncDice.OpenDice((value) => {
            callback?.Invoke(value + extraDicePoint);
            extraDicePoint = 0;
        });

        return dice;
    }

    public GUISelectMovePawn SelectMovePawn(int value)
    {
        return _syncMovePawn.OpenSelectMovePawn(value);
    }

}