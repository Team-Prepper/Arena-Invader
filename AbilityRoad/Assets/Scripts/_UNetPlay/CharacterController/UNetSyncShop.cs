using System;
using Unity.Netcode;
using UnityEngine;

public class UNetSyncShop : NetworkBehaviour {

    NetworkSyncUIConnector<GUIShop, int> _shopSync;
    ICharacterController _cc;

    public void Initial(ICharacterController cc)
    {
        _cc = cc;
        _shopSync = new NetworkSyncUIConnector<GUIShop, int>("Shop");
    }

    public GUIShop OpenShop(Action callback)
    {
        GUIShop shop = _shopSync.ControlClientOpen();

        int seed = ItemManager.Instance.RandomItemListByInt(shop.Size);

        shop.SetBuyer(_cc);
        shop.SetItems(seed);

        shop.SetCloseMethod(() =>
        {
            callback?.Invoke();
            CloseShopServerRpc();
        });

        shop.NetworkModifiedMethodSet(ShopValueChangeServerRpc);

        OpenShopServerRpc(seed);

        return shop;

    }

    [ServerRpc(RequireOwnership = false)]
    void ShopValueChangeServerRpc(int value) => ShopValueChangeClientRpc(value);

    [ClientRpc]
    void ShopValueChangeClientRpc(int value)
    {
        _shopSync.SetModified(value);
    }

    [ServerRpc(RequireOwnership = false)]
    void OpenShopServerRpc(int seed) => OpenShopClientRpc(seed);

    [ClientRpc]
    void OpenShopClientRpc(int seed)
    {
        _shopSync.ClientOpen((ui) => { 
            ui.SetItems(seed);
        });
    }


    [ServerRpc(RequireOwnership = false)]
    void CloseShopServerRpc() => CloseShopClientRpc();

    [ClientRpc]
    void CloseShopClientRpc() => _shopSync.Close();

}