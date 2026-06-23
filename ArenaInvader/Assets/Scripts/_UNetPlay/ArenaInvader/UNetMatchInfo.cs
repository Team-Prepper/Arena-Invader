using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using EasyH.Unity.UI;

public class UNetMatchInfo : NetworkBehaviour, IMatchInfo, INetworkMatchInfo {

    public struct SimplePlayerInfor : INetworkSerializable, IEquatable<SimplePlayerInfor> {

        public ulong ClientId;
        public FixedString32Bytes Name;
        public FixedString32Bytes CharacterCode;
        public bool IsAI;

        public SimplePlayerInfor(ulong clientId, string name, string cc, bool isAI = false)
        {
            ClientId = clientId;
            Name = name;
            CharacterCode = cc;
            IsAI = isAI;
        }

        public bool Equals(SimplePlayerInfor other)
        {
            if (other.ClientId != ClientId) return false;
            if (!other.Name.Equals(Name)) return false;
            if (!other.CharacterCode.Equals(CharacterCode)) return false;
            if (other.IsAI != IsAI) return false;
            return true;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref ClientId);
            serializer.SerializeValue(ref Name);
            serializer.SerializeValue(ref CharacterCode);
            serializer.SerializeValue(ref IsAI);
        }

    }

    [SerializeField] private NetworkVariable<bool> IsMatchStart
        = new NetworkVariable<bool>(false);

    private NetworkVariable<UNetString> NetMatchDice =
        new NetworkVariable<UNetString>(new UNetString("DartDice"));

    private NetworkVariable<UNetString> NetMapName =
        new NetworkVariable<UNetString>(new UNetString("Map/DefaultMap"));

    private NetworkList<SimplePlayerInfor> NetworkPlayerInfor;

    public IList<PlayerInfor> PlayerInfors { get; private set; }

    public string MapName => NetMapName.Value.ToString();

    public string MatchDice => NetMatchDice.Value.ToString();

    public IList<int> EditableIdx {
        get {
            int editableIdx = GetPlayerIdx(NetworkManager.Singleton.LocalClientId);
            IList<int> retval = new List<int>
            {
                editableIdx
            };
            return retval;
        }
    }

    private IGUI _gui;
    private bool _subscribedToDisconnect;

    public void SetMatchSettingUI(IGUI gui) {
        _gui = gui;
    }
    
    public void OpenSettingUI()
    {
        UIManager.Instance.OpenGUI<GUIWindow>("UNetMatchInforSetting");
    }

    void Awake() {
        NetworkPlayerInfor = new NetworkList<SimplePlayerInfor>();

        PlayerInfors = new PlayerInfor[0];
    }

    public override void OnNetworkSpawn()
    {
        if (IsMatchStart.Value == true) {
            Dispose();
            return;
        }

        GameManager.Instance.MatchInfo = this;
        GameManager.Instance.OnMatchInfoChanged?.Invoke();

        NetMapName.OnValueChanged += OnMapNameChanged;
        NetMatchDice.OnValueChanged += OnMatchDiceChanged;
        NetworkPlayerInfor.OnListChanged += OnNetworkPlayerInforChanged;

        AddNewPlayerServerRpc(string.Format("Player {0}", NetworkManager.Singleton.LocalClientId), "Player");

        if (!IsHost) return;

        NetworkManager.Singleton.OnClientDisconnectCallback += ServerClientQuit;
        _subscribedToDisconnect = true;

    }

    private void OnMapNameChanged(UNetString beforeValue, UNetString value)
    {
        GameManager.Instance.OnMatchInfoChanged?.Invoke();
    }

    private void OnMatchDiceChanged(UNetString beforeValue, UNetString value)
    {
        GameManager.Instance.OnMatchInfoChanged?.Invoke();
    }

    private void OnNetworkPlayerInforChanged(NetworkListEvent<SimplePlayerInfor> eve)
    {
        NetPlayerInforToPlayerInfor();
    }

    private void ServerClientQuit(ulong clientId)
    {
        int playerIdx = GetPlayerIdx(clientId);
        if (playerIdx < 0 || playerIdx >= NetworkPlayerInfor.Count)
        {
            return;
        }

        NetworkPlayerInfor.RemoveAt(playerIdx);

    }

    [ServerRpc(RequireOwnership = false)]
    private void AddNewPlayerServerRpc(
        string name,
        string cc,
        ServerRpcParams serverRpcParams = default)
    {
        NetworkPlayerInfor.Add(new SimplePlayerInfor(
            serverRpcParams.Receive.SenderClientId,
            name,
            cc));

    }

    public void StartMatch() {
        if (!IsHost) return;
        if (PlayerInfors.Count < 2)
        {
            UIManager.Instance.DisplayMessage("msg_NeedMorePlayer");
            return;
        }
        IsMatchStart.Value = true;
        GameManager.Instance.Playground.StartMatch();
    }

    public void Dispose() {
        _gui?.Close();
        UnsubscribeEvents();
        if (NetManager.Instance?.System != null)
        {
            NetManager.Instance.System.Disconnect();
        }
    }

    private void UnsubscribeEvents()
    {
        NetMapName.OnValueChanged -= OnMapNameChanged;
        NetMatchDice.OnValueChanged -= OnMatchDiceChanged;
        if (NetworkPlayerInfor != null)
        {
            NetworkPlayerInfor.OnListChanged -= OnNetworkPlayerInforChanged;
        }

        if (IsHost && _subscribedToDisconnect)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= ServerClientQuit;
            _subscribedToDisconnect = false;
        }
    }

    private void NetPlayerInforToPlayerInfor()
    {
        PlayerInfors = new PlayerInfor[NetworkPlayerInfor.Count];

        for (int i = 0; i < PlayerInfors.Count; i++)
        {
            PlayerInfors[i] = new PlayerInfor(NetworkPlayerInfor[i].Name.ToString(),
                NetworkPlayerInfor[i].CharacterCode.ToString(), NetworkPlayerInfor[i].IsAI);
        }

        GameManager.Instance.OnMatchInfoChanged?.Invoke();
    }


    public override void OnNetworkDespawn()
    {
        UnsubscribeEvents();
        base.OnNetworkDespawn();
    }

    public void SetPlayerCnt(int cnt)
    {
        Debug.LogWarning(
            $"{nameof(UNetMatchInfo)} does not support runtime player count changes. " +
            "Player slots follow connected network clients.");
    }

    public void SetDice(string diceCode)
    {
        NetMatchDice.Value = new UNetString(diceCode);

        GameManager.Instance.OnMatchInfoChanged?.Invoke();
    }

    public void SetPlayerName(int idx, string name)
    {
        SetPlayerNameServerRpc(idx, name);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetPlayerNameServerRpc(int idx, string name)
    {
        if (idx >= PlayerInfors.Count) return;
        SimplePlayerInfor def = NetworkPlayerInfor[idx];
        NetworkPlayerInfor[idx] = new SimplePlayerInfor(
            def.ClientId,
            name,
            def.CharacterCode.ToString(),
            def.IsAI);

    }

    public void SetPlayerCharacter(int idx, string name)
    {
        SetPlayerCharacterServerRpc(idx, name);
    }

    public void SetPlayerIsAI(int idx, bool isAI)
    {
        SetPlayerIsAIServerRpc(idx, isAI);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetPlayerCharacterServerRpc(int idx, string name)
    {
        if (idx >= PlayerInfors.Count) return;
        SimplePlayerInfor def = NetworkPlayerInfor[idx];
        NetworkPlayerInfor[idx] = new SimplePlayerInfor(
            def.ClientId,
            def.Name.ToString(),
            name,
            def.IsAI);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetPlayerIsAIServerRpc(int idx, bool isAI)
    {
        if (idx >= PlayerInfors.Count) return;
        SimplePlayerInfor def = NetworkPlayerInfor[idx];
        NetworkPlayerInfor[idx] = new SimplePlayerInfor(
            def.ClientId,
            def.Name.ToString(),
            def.CharacterCode.ToString(),
            isAI);
    }

    public void SetMap(string mapName)
    {
        NetMapName.Value = new UNetString(mapName);
        GameManager.Instance.OnMatchInfoChanged?.Invoke();
    }

    public int GetPlayerIdx(ulong clientId)
    {
        for (int i = 0; i < NetworkPlayerInfor.Count; i++)
        {
            if (NetworkPlayerInfor[i].ClientId == clientId)
            {
                return i;
            }
        }

        return -1;
    }

    public ulong GetClientId(int playerIdx)
    {
        if (playerIdx < 0 || playerIdx >= NetworkPlayerInfor.Count)
        {
            return NetworkManager.ServerClientId;
        }

        return NetworkPlayerInfor[playerIdx].ClientId;
    }

}
