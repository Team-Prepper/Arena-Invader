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
        _diceSync = new ServerClientSyncGUI<GUIDice>(GameManager.Instance.MatchInfor.MatchDice);
        _selectMovePawnSync = new ServerClientSyncGUI<GUISelectMovePawn>("SelectMovePawn");
        _inventorySync = new ServerClientSyncGUI<GUIOpenInventory>("Inventory");
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
            CloseShopServerRpc();
        });

        EnterShopServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    void EnterShopServerRpc() => EnterShopClientRpc();

    [ServerRpc(RequireOwnership = false)]
    void CloseShopServerRpc() => CloseShopClientRpc();

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
            CloseDiceServerRpc();
        });

        OpenDiceServerRpc();

        return dice;
    }

    [ServerRpc(RequireOwnership = false)]
    void OpenDiceServerRpc() => OpenDiceClientRpc();

    [ServerRpc(RequireOwnership = false)]
    void CloseDiceServerRpc() => CloseDiceClientRpc();
    [ClientRpc(RequireOwnership=false)]
    void OpenDiceClientRpc() => _diceSync.ClientOpen();

    [ClientRpc(RequireOwnership = false)]
    void CloseDiceClientRpc() => _diceSync.Close();

    public GUISelectMovePawn SelectMovePawn(int value)
    {
        GUISelectMovePawn movePawn = _selectMovePawnSync.Client¾îÂ¼±¸();

        Target.OnPawnChoose();

        movePawn.SetPlayer(this, value, () => {
            Target.OffPawnChoose();
            CloseSelectMovePawnServerRpc();
        });

        OpenSelectMovePawnServerRpc();

        return movePawn;
    }

    [ServerRpc(RequireOwnership = false)]
    void OpenSelectMovePawnServerRpc() => OpenSelectMovePawnClientRpc();

    [ServerRpc(RequireOwnership = false)]
    void CloseSelectMovePawnServerRpc() => CloseSelectMovePawnClientRpc();

    [ClientRpc]
    void OpenSelectMovePawnClientRpc() => _selectMovePawnSync.ClientOpen();

    [ClientRpc]
    void CloseSelectMovePawnClientRpc() => _selectMovePawnSync.Close();

}