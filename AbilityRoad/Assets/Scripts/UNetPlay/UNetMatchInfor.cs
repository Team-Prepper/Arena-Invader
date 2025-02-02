using EHTool.UIKit;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class UNetMatchInfor : NetworkBehaviour, IMatchInfor {

    public struct SimplePlayerInfor : INetworkSerializable, IEquatable<SimplePlayerInfor> {

        public FixedString32Bytes Name;
        public FixedString32Bytes CharacterCode;

        public SimplePlayerInfor(string name, string cc)
        {
            Name = name;
            CharacterCode = cc;
        }

        public bool Equals(SimplePlayerInfor other)
        {
            if (!other.Name.Equals(Name)) return false;
            if (!other.CharacterCode.Equals(CharacterCode)) return false;
            return true;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Name);
            serializer.SerializeValue(ref CharacterCode);
        }

    }

    NetworkVariable<UNetString> NetMatchDice =
        new NetworkVariable<UNetString>(new UNetString("DartDice"));

    NetworkVariable<UNetString> NetMapName =
        new NetworkVariable<UNetString>(new UNetString("Map/DefaultMap"));

    NetworkList<SimplePlayerInfor> NetworkPlayerInfor;

    PlayerInfor[] _playerInfors;

    public IList<PlayerInfor> PlayerInfors
        => _playerInfors;

    string IMatchInfor.MapName
        => NetMapName.Value.ToString();

    string IMatchInfor.MatchDice
        => NetMatchDice.Value.ToString();

    public void OpenSettingUI()
    {
        UIManager.Instance.OpenGUI<GUIWindow>("UNetMatchInforSetting");
    }

    void Awake() {
        NetworkPlayerInfor = new NetworkList<SimplePlayerInfor>();
        GameManager.Instance.OnMatchInforChanged?.Invoke();
    }

    public override void OnNetworkSpawn()
    {
        GameManager.Instance.MatchInfor = this;

        NetMapName.OnValueChanged += (beforeValue, value) =>
        {
            GameManager.Instance.OnMatchInforChanged?.Invoke();
        };

        NetMatchDice.OnValueChanged += (befeoreValue, value) =>
        {
            GameManager.Instance.OnMatchInforChanged?.Invoke();
        };

        NetworkPlayerInfor.OnListChanged += (eve) =>
        {
            NetPlayerInforToPlayerInfor();
        };

        AddNewPlayerServerRpc(string.Format("Player {0}", NetworkManager.Singleton.LocalClientId), "Player");

        if (!IsHost) return;

        NetworkManager.Singleton.OnClientDisconnectCallback += (clientId) =>
        {
            Debug.Log(clientId);
            for (int i = 0; i < NetworkManager.Singleton.ConnectedClientsIds.Count; i++)
            {
                if (NetworkManager.Singleton.ConnectedClientsIds.ElementAt(i) < clientId)
                    continue;
                NetworkPlayerInfor.RemoveAt(i);
                return;
            }
            NetworkPlayerInfor.RemoveAt(NetworkManager.Singleton.ConnectedClientsIds.Count);
        };
    }

    [ServerRpc(RequireOwnership = false)]
    void AddNewPlayerServerRpc(string name, string cc)
    {
        NetworkPlayerInfor.Add(new SimplePlayerInfor(name, cc));

    }

    void NetPlayerInforToPlayerInfor()
    {
        _playerInfors = new PlayerInfor[NetworkPlayerInfor.Count];

        for (int i = 0; i < _playerInfors.Length; i++)
        {
            _playerInfors[i] = new PlayerInfor(NetworkPlayerInfor[i].Name.ToString(),
                NetworkPlayerInfor[i].CharacterCode.ToString(), false);
        }

        GameManager.Instance.OnMatchInforChanged?.Invoke();
    }


    public override void OnNetworkDespawn()
    {
        base.OnNetworkSpawn();
        NetMapName.OnValueChanged = null;
    }

    public IList<int> EditableIdx {
        get {
            IList<int> retval = new List<int>();
            for (int i = 0; i < _playerInfors.Length; i++)
            {
                retval.Add(i);
            }
            return retval;
        }
    }

    public void SetPlayerCnt(int cnt)
    {

    }

    public void SetDice(string diceCode)
    {
        NetMatchDice.Value = new UNetString(diceCode);

        GameManager.Instance.OnMatchInforChanged?.Invoke();
    }

    public void SetPlayerName(int idx, string name)
    {
        if (idx >= _playerInfors.Length) return;
        SimplePlayerInfor def = NetworkPlayerInfor[idx];
        NetworkPlayerInfor[idx] = new SimplePlayerInfor(name, def.CharacterCode.ToString());
    }

    public void SetPlayerCharacter(int idx, string name)
    {
        if (idx >= _playerInfors.Length) return;
        SimplePlayerInfor def = NetworkPlayerInfor[idx];
        NetworkPlayerInfor[idx] = new SimplePlayerInfor(def.Name.ToString(), name);
    }

    public void SetMap(string mapName)
    {
        NetMatchDice.Value = new UNetString(mapName);
        GameManager.Instance.OnMatchInforChanged?.Invoke();
    }
}
