using Unity.Netcode;

public class UNetSyncInventory : NetworkBehaviour {

    NetworkSyncUIConnector<GUIOpenInventory, int> _inventorySync;
    ICharacterController _cc;

    public void Initial(ICharacterController cc)
    {
        _cc = cc;
        _inventorySync =
            new NetworkSyncUIConnector<GUIOpenInventory, int>("Inventory");
    }

    public GUIOpenInventory OpenInventory()
    {
        GUIOpenInventory inventory = _inventorySync.ControlClientOpen();

        int value = ItemManager.Instance.ItemListToInt(_cc.Status.Items);
        int size = _cc.Status.Items.Count;

        inventory.SetTarget(value, size, _cc);

        inventory.SetCloseMethod(() =>
        {
            CloseInventoryServerRpc();
        });

        inventory.NetworkModifiedMethodSet(InventoryValueChangeServerRpc);

        InventoryMovePawnServerRpc(value, size);

        return inventory;
    }

    [ServerRpc(RequireOwnership = false)]
    void InventoryValueChangeServerRpc(int value) => InventoryValueChangeClientRpc(value);

    [ClientRpc]
    void InventoryValueChangeClientRpc(int value)
    {
        _inventorySync.SetModified(value);
    }

    [ServerRpc(RequireOwnership = false)]
    void InventoryMovePawnServerRpc(int value, int size) => OpenInventoryClientRpc(value, size);

    [ClientRpc]
    void OpenInventoryClientRpc(int value, int size)
    {
        _inventorySync.ClientOpen((ui) => {
            ui.SetTarget(value, size);
        });
    }

    [ServerRpc(RequireOwnership = false)]
    void CloseInventoryServerRpc() => CloseInventoryClientRpc();

    [ClientRpc]
    void CloseInventoryClientRpc() => _inventorySync.Close();

}