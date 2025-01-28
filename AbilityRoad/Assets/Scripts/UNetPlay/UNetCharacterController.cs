using EHTool.UIKit;
using System;
using Unity.Netcode;
using UnityEngine;

public class UNetCharacterController : NetworkBehaviour, ICharacterController {
    
    public BasePlayer Target { get; private set; }
    ICharacterActionSelector _selector;

    [SerializeField] private int _chance = 0;
    private int extraDicePoint = 0;

    ServerClientSyncGUI<GUIShop> _shopSync;
    ServerClientSyncGUI<GUIDice> _diceSync;
    ServerClientSyncGUI<GUISelectMovePawn> _selectMovePawnSync;
    ServerClientSyncGUI<GUIOpenInventory> _inventorySync;

    void Start() {
        _shopSync = new ServerClientSyncGUI<GUIShop>("Shop");
        _diceSync = new ServerClientSyncGUI<GUIDice>(GameManager.Instance.Playground.MatchInfor.MatchDice);
        _selectMovePawnSync = new ServerClientSyncGUI<GUISelectMovePawn>("SelectMovePawn");
        _inventorySync = new ServerClientSyncGUI<GUIOpenInventory>("Inventory");
    }

    public void SetMatch(ICharacterActionSelector selector) {

        _selector = selector;
        SetMatchServerRpc();

    }
    public void SetTargetCharacter(string name, string characterCode, int idx)
    {
        SetTargetCharacterClientRpc(characterCode, name, idx);
    }

    [ServerRpc]
    void SetMatchServerRpc() {
        GameManager.Instance.Playground.AddPlayer(this);
    }

    [ClientRpc]
    void SetTargetCharacterClientRpc(string name, string characterCode, int idx) {
        Target = CharacterManager.Instance.SpawnPlayer(characterCode);
        Target.SetInitial(this, name, idx);
    }

    public void StartTurn()
    {
        StartTurnClientRpc();
    }

    [ClientRpc]
    void StartTurnClientRpc()
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
        MovePawnClientRpc(pawnId, amount);
    }

    [ClientRpc]
    public void MovePawnClientRpc(int pawnId, int amount)
    {
        Target._pawns[pawnId].Move(amount);

    }

    public void EndTurn()
    {
        if (_chance == 0)
        {
            EndTurnServerRpc();
            return;
        }
        _selector.StartTurn(this);
    }

    [ServerRpc]
    void EndTurnServerRpc() {
        GameManager.Instance.Playground.TurnEnd();
    }

    public void AddChance()
    {
        _chance++;
    }

    public void GetExtraDicePoint(int point)
    {
        extraDicePoint += point;
    }

    public GUIOpenInventory OpenInventory(Action<int> value)
    {
        GUIOpenInventory inventory = _inventorySync.Client¾îÂ¼±¸();
        return inventory;
    }

    public void EnterShop(Action callback)
    {
        GUIShop shop = _shopSync.Client¾îÂ¼±¸();

        shop.EnterShop(Target, () =>
        {
            callback?.Invoke();
            CloseShopClientRpc();
        });

        EnterShopClientRpc();
    }

    [ClientRpc]
    void EnterShopClientRpc() => _shopSync.ClientOpen();

    [ClientRpc]
    void CloseShopClientRpc() => _shopSync.Close();

    public GUIDice RollDice(Action<int> callback)
    {
        _chance--;

        GUIDice dice = _diceSync.Client¾îÂ¼±¸();

        dice.SetCallback((value) => {
            callback?.Invoke(value + extraDicePoint);
            extraDicePoint = 0;
            CloseDiceClientRpc();
        });

        OpenDiceClientRpc();

        return dice;
    }

    [ClientRpc]
    void OpenDiceClientRpc() => _diceSync.ClientOpen();

    [ClientRpc]
    void CloseDiceClientRpc() => _diceSync.Close();

    public GUISelectMovePawn SelectMovePawn(int value)
    {
        GUISelectMovePawn movePawn = _selectMovePawnSync.Client¾îÂ¼±¸();

        Target.OnPawnChoose();

        movePawn.SetPlayer(this, value, () => {
            Target.OffPawnChoose();
            CloseSelectMovePawnClientRpc();
        });

        OpenSelectMovePawnClientRpc();

        return movePawn;
    }

    [ClientRpc]
    void OpenSelectMovePawnClientRpc() => _selectMovePawnSync.ClientOpen();

    [ClientRpc]
    void CloseSelectMovePawnClientRpc() => _selectMovePawnSync.Close();

}