using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using EasyH.Gaming.TurnBased;

public class UNetMemberState : NetworkBehaviour, IMemberState
{
    public bool TurnEnd { get; private set; } = true;

    public int TeamIdx { get; private set; } = -1;

    public Action<bool> OnTurnEndStateChanged { get; set; }
    public Action OnTeamIdxChanged { get; set; }

    public void SetTeamIdx(int idx)
    {
        SetTeamIdxServerRpc(idx);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetTeamIdxServerRpc(int idx)
    {
        SetTeamIdxClientRpc(idx);
    }

    [ClientRpc]
    public void SetTeamIdxClientRpc(int idx)
    {
        if (idx == TeamIdx) return;

        if (TeamIdx >= 0)
        {
            TurnManager.Instance.System.RemoveTeamMember(this);
        }

        TeamIdx = idx;
        OnTeamIdxChanged?.Invoke();
        TurnManager.Instance.System.AddTeamMember(this);

    }

    public void Remove()
    {
        TurnManager.Instance.System.RemoveTeamMember(this);

    }

    public void StartTurn()
    {
        TurnEnd = false;
        OnTurnEndStateChanged?.Invoke(false);
    }

    public void EndTurn()
    {
        EndTurnServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void EndTurnServerRpc()
    {
        EndTurnClientRpc();
    }

    [ClientRpc]
    private void EndTurnClientRpc()
    {
        TurnEnd = true;
        TurnManager.Instance.System.TurnEnd();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        NewSystemJoinServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void NewSystemJoinServerRpc()
    {
        SetTeamIdxClientRpc(TeamIdx);
    }

}
