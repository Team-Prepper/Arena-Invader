using System;
using Unity.Netcode;
using UnityEngine;

public class UNetCharacterController : NetworkBehaviour, ICharacterController {
    
    public int PlayerId { get; set; }
    [SerializeField] private UNetStatus _status;

    public IStatus Status => _status;
    public BasePlayer Target { get; private set; }

    private ICharacterActionSelector _selector;

    [SerializeField] private int _chance = 0;
    private int extraDicePoint = 0;

    [SerializeField] private UNetSyncShop _syncShop;
    [SerializeField] private UNetSyncMovePawn _syncMovePawn;
    [SerializeField] private UNetSyncDice _syncDice;
    [SerializeField] private UNetSyncInventory _syncInventory;

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
        _status = gameObject.GetComponent<UNetStatus>();

        Status.Name = name;
        Status.CharacterCode = characterCode;
        Status.SetCC(this);
        
        SetTargetCharacterClientRpc(name, characterCode, idx);
    }

    [ClientRpc]
    void SetTargetCharacterClientRpc(string name, string characterCode, int idx) {

        PlayerId = idx;
        GameManager.Instance.Playground.AddPlayer(this);
        
        _status = gameObject.GetComponent<UNetStatus>();
        Status.Name = name;
        Status.CharacterCode = characterCode;

        Target = CharacterManager.Instance.SpawnPlayer(characterCode);
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

    public GUIOpenInventory OpenInventory()
    {
        GUIOpenInventory inventory = _syncInventory.OpenInventory();
        inventory.SetCloseMethod(_selector.RollDice);
        return inventory;
    }

    public void OpenShop(Action callback)
    {
        if (_selector == null) return;
        _syncShop.OpenShop(callback);
        
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