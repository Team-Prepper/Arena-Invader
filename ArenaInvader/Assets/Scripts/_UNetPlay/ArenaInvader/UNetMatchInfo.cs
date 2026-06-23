using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using EasyH.Unity.UI;

public class UNetMatchInfo : NetworkBehaviour, IMatchInfo {

    public struct SimplePlayerInfor : INetworkSerializable, IEquatable<SimplePlayerInfor> {

        public FixedString32Bytes Name;
        public FixedString32Bytes CharacterCode;
        public bool IsAI;

        public SimplePlayerInfor(string name, string cc, bool isAI = false)
        {
            Name = name;
            CharacterCode = cc;
            IsAI = isAI;
        }

        public bool Equals(SimplePlayerInfor other)
        {
            if (!other.Name.Equals(Name)) return false;
            if (!other.CharacterCode.Equals(CharacterCode)) return false;
            if (other.IsAI != IsAI) return false;
            return true;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
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
            IList<int> retval = new List<int>
            {
                NetManager.Instance.System.Id
            };
            return retval;
        }
    }

    private IGUI _gui;

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

        NetMapName.OnValueChanged += (beforeValue, value) =>
        {
            GameManager.Instance.OnMatchInfoChanged?.Invoke();
        };

        NetMatchDice.OnValueChanged += (befeoreValue, value) =>
        {
            GameManager.Instance.OnMatchInfoChanged?.Invoke();
        };

        NetworkPlayerInfor.OnListChanged += (eve) =>
        {
            NetPlayerInforToPlayerInfor();
        };

        AddNewPlayerServerRpc(string.Format("Player {0}", NetworkManager.Singleton.LocalClientId), "Player");

        if (!IsHost) return;

        NetworkManager.Singleton.OnClientDisconnectCallback += ServerClientQuit;

    }

    private void ServerClientQuit(ulong clientId)
    {
        Debug.Log(clientId);
        NetworkPlayerInfor.RemoveAt(NetManager.Instance.System.GetIdx(clientId));

    }

    [ServerRpc(RequireOwnership = false)]
    private void AddNewPlayerServerRpc(string name, string cc)
    {
        NetworkPlayerInfor.Add(new SimplePlayerInfor(name, cc));

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
        if (IsHost)
            NetworkManager.Singleton.OnClientDisconnectCallback -= ServerClientQuit;
        NetManager.Instance.System.Disconnect();
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
        base.OnNetworkSpawn();
        NetMapName.OnValueChanged = null;
    }

    public void SetPlayerCnt(int cnt)
    {
        
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
        NetworkPlayerInfor[idx] = new SimplePlayerInfor(name, def.CharacterCode.ToString());

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
        NetworkPlayerInfor[idx] = new SimplePlayerInfor(def.Name.ToString(), name, def.IsAI);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetPlayerIsAIServerRpc(int idx, bool isAI)
    {
        if (idx >= PlayerInfors.Count) return;
        SimplePlayerInfor def = NetworkPlayerInfor[idx];
        NetworkPlayerInfor[idx] = new SimplePlayerInfor(def.Name.ToString(), def.CharacterCode.ToString(), isAI);
    }

    public void SetMap(string mapName)
    {
        NetMapName.Value = new UNetString(mapName);
        GameManager.Instance.OnMatchInfoChanged?.Invoke();
    }

}
