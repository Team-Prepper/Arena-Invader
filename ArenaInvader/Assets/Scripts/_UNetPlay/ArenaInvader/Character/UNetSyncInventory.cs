using Unity.Netcode;
using System;

public class UNetSyncInventory : NetworkBehaviour, IOpenInventory {

    private NetworkSyncUIConnector<GUIInventory, int> _inventorySync;
    private IPlayableCharacter _cc;
    private IInventory _inventory;
    private Action _inventoryChangedHandler;

    public void Initial(IPlayableCharacter cc)
    {
        _cc = cc;
        _inventory = cc.Inventory;
        _inventorySync =
            new NetworkSyncUIConnector<GUIInventory, int>("Inventory");
        _inventoryChangedHandler = RefreshOpenInventory;
        _inventory.OnItemsChanged += _inventoryChangedHandler;
    }

    private void OnDestroy()
    {
        if (_inventory != null && _inventoryChangedHandler != null)
        {
            _inventory.OnItemsChanged -= _inventoryChangedHandler;
        }
    }

    public GUIInventory OpenInventory()
    {
        GUIInventory inventory = _inventorySync.ControlClientOpen();

        int value = ItemManager.Instance.ItemListToInt(_inventory.Items);
        int size = _inventory.Items.Count;

        inventory.SetTarget(value, size, _cc);

        inventory.SetCloseMethod(() =>
        {
            CloseInventoryServerRpc();
        });

        inventory.NetworkModifiedMethodSet(InventoryValueChangeServerRpc);

        InventoryMovePawnServerRpc(value, size);

        return inventory;
    }

    private void RefreshOpenInventory()
    {
        int value = ItemManager.Instance.ItemListToInt(_inventory.Items);
        int size = _inventory.Items.Count;

        if (_inventorySync.IsOpen && _inventorySync.CurrentGUI != null)
        {
            _inventorySync.CurrentGUI.SetTarget(value, size, _cc);
        }
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
    
    
    public void ShowUseItem(string itemCode)
    {
        ShowUseItemServerRpc(itemCode);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ShowUseItemServerRpc(string itemCode)
        => ShowUseItemClientRpc(itemCode);

    [ClientRpc]
    public void ShowUseItemClientRpc(string itemCode)
    {
        EasyH.Unity.UI.UIManager.Instance.
            OpenGUI<GUIUseItemShow>("UseItemShow").SetUseItem(itemCode);
    }

}
