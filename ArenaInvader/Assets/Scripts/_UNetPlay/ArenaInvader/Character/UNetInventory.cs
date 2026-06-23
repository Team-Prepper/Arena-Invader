using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.Collections;
using Unity.Netcode;

public class UNetInventory : NetworkBehaviour, IInventory
{
    public Action OnItemsChanged { get; set; }

    private readonly List<string> _items = new List<string>();
    private NetworkList<FixedString32Bytes> _netItems;

    public IList<string> Items => _items;

    private IPlayableCharacter _cc;

    private void Awake()
    {
        _netItems = new NetworkList<FixedString32Bytes>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        _netItems.OnListChanged += OnNetworkItemsChanged;
        SyncItemsFromNetworkList();
    }

    public override void OnNetworkDespawn()
    {
        if (_netItems != null)
        {
            _netItems.OnListChanged -= OnNetworkItemsChanged;
        }

        base.OnNetworkDespawn();
    }

    public void SetCC(IPlayableCharacter cc)
    {
        _cc = cc;
    }

    public void AddItem(string item)
    {
        if (IsServer)
        {
            AddItemInternal(item);
            return;
        }

        AddItemServerRpc(item);
    }

    [ServerRpc(RequireOwnership = false)]
    private void AddItemServerRpc(string item)
    {
        AddItemInternal(item);
    }

    public void UseItem(string item)
    {
        if (IsServer)
        {
            UseItemInternal(item);
            return;
        }

        UseItemServerRpc(item);
    }

    [ServerRpc(RequireOwnership = false)]
    private void UseItemServerRpc(string item)
    {
        UseItemInternal(item);
    }

    public void DiscardItem(string item)
    {
        if (IsServer)
        {
            DiscardItemInternal(item);
            return;
        }

        DiscardItemServerRpc(item);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DiscardItemServerRpc(string item)
    {
        DiscardItemInternal(item);
    }

    private void AddItemInternal(string item)
    {
        if (string.IsNullOrEmpty(item))
        {
            return;
        }

        _netItems.Add(new FixedString32Bytes(item));
    }

    private void UseItemInternal(string item)
    {
        int idx = FindItemIndex(item);
        if (idx < 0)
        {
            return;
        }

        ItemManager.Instance.GetItemData(item).Item.UseItem(_cc);
        _netItems.RemoveAt(idx);
    }

    private void DiscardItemInternal(string item)
    {
        int idx = FindItemIndex(item);
        if (idx < 0)
        {
            return;
        }

        _netItems.RemoveAt(idx);
    }

    private int FindItemIndex(string item)
    {
        for (int i = 0; i < _netItems.Count; i++)
        {
            if (_netItems[i].ToString() == item)
            {
                return i;
            }
        }

        return -1;
    }

    private void OnNetworkItemsChanged(NetworkListEvent<FixedString32Bytes> eve)
    {
        SyncItemsFromNetworkList();
    }

    private void SyncItemsFromNetworkList()
    {
        _items.Clear();

        for (int i = 0; i < _netItems.Count; i++)
        {
            _items.Add(_netItems[i].ToString());
        }

        OnItemsChanged?.Invoke();
    }
}
