using EHTool;
using System;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class UNetNetwork : MonoBehaviour, INetwork {

    NetworkManager _uNetManager;

    public int Id {
        get {
            for (int i = 0; i < _uNetManager.ConnectedClientsIds.Count; i++) {
                if (_uNetManager.ConnectedClientsIds.ElementAt(i) != _uNetManager.LocalClientId)
                    continue;
                return i;
            }
            return _uNetManager.ConnectedClientsIds.Count;
        }
    }

    void Awake() {
        _uNetManager = AssetOpener.ImportComponent<NetworkManager>("NetworkManager");
        /*
        NetworkConfig;
        NetworkPrefabs;
        NetworkPrefabsList;
        */
    }

    public void Disconnect() {
        _uNetManager.Shutdown();
        GameManager.Instance.MatchInfor = new LocalMatchInfor(2, "Map/DefaultMap", "DartDice");
    }

    public void StartHost()
    {
        _uNetManager.StartHost();
        GameManager.Instance.MatchInfor = null;

        AssetOpener.ImportComponent<NetworkObject>("UNetPlayground").Spawn();
    }

    public void StartClient()
    {
        _uNetManager.StartClient();
        GameManager.Instance.MatchInfor = null;
    }

}