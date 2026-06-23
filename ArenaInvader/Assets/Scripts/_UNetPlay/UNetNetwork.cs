using EHTool;
using Unity.Netcode;
using UnityEngine;

public class UNetNetwork : MonoBehaviour, INetwork {

    private NetworkManager _uNetManager;

    public int Id => GetIdx(_uNetManager.LocalClientId);
    
    public int GetIdx(ulong clientId) {
        
        for (int i = 0; i < _uNetManager.ConnectedClientsIds.Count; i++)
        {
            if (_uNetManager.ConnectedClientsIds[i] != clientId)
                continue;
            return i;
        }
        return _uNetManager.ConnectedClientsIds.Count;

    }

    public static void OnNetwork()
    {
        if (NetManager.Instance.System != null) return;

        NetManager.Instance.System =
            NetManager.Instance.gameObject.
                AddComponent<UNetNetwork>();
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
        GameManager.Instance.MatchInfo = null;
    }

    public void StartHost()
    {
        _uNetManager.StartHost();
        GameManager.Instance.MatchInfo = null;

        AssetOpener.ImportComponent<NetworkObject>(
            "UNetPlayground").Spawn();
    }

    public void StartClient()
    {
        _uNetManager.StartClient();
        GameManager.Instance.MatchInfo = null;
    }

}
