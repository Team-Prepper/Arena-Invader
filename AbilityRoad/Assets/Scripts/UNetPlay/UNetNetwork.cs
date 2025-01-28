using EHTool;
using Unity.Netcode;
using UnityEngine;

public class UNetNetwork : MonoBehaviour, INetwork {

    NetworkManager _uNetManager;

    void Awake() {
        _uNetManager = AssetOpener.ImportComponent<NetworkManager>("NetworkManager");
        /*
        NetworkConfig;
        NetworkPrefabs;
        NetworkPrefabsList;
        */
    }

    public void StartHost() => _uNetManager.StartHost();
    public void StartClient() => _uNetManager.StartClient();

}