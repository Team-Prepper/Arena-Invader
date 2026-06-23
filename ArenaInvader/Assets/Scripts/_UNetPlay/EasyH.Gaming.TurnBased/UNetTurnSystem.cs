using System;
using System.Collections.Generic;
using EasyH;
using EasyH.Gaming.TurnBased;
using Unity.Netcode;

public class UNetTurnSystem : NetworkBehaviour, ITurnSystem
{

    private Func<bool> _condition;
    private int _turn;
    private IList<Team> _teams;
    private ISet<IObserver<int>> _observers;

    public int TurnSpend => _teams.Count > 0 ? _turn / _teams.Count : 0;
    public int ActiveTeamIdx => _teams.Count > 0 ? _turn % _teams.Count : 0;

    private void Awake()
    {
        _teams = new List<Team>();
        _observers = new HashSet<IObserver<int>>();

    }

    public void AddTeamMember(IMemberState m)
    {
        while (m.TeamIdx >= _teams.Count)
        {
            _teams.Add(new Team());
        }
        _teams[m.TeamIdx].AddMember(m);
    }

    public void RemoveTeamMember(IMemberState m)
    {
        if (m.TeamIdx < 0 || m.TeamIdx >= _teams.Count)
        {
            return;
        }

        _teams[m.TeamIdx].RemoveMember(m);

        if (!IsServer) return;
        if (_teams[m.TeamIdx].GetLeftMemberCount() > 0) return;

        TeamRetire(m.TeamIdx);
        TurnEnd();
    }

    public void SetGameProceedCondition(Func<bool> condition)
    {
        _condition = condition;
    }

    public void StartGame()
    {
        if (!IsServer) return;
        if (_teams.Count == 0) return;
        _turn = 0;
        StartTurnClientRpc(_turn);
    }

    public void TurnEnd()
    {
        if (!IsServer) return;
        if (_teams.Count == 0) return;
        if (_condition != null && !_condition()) return;

        _turn++;

        int remainingChecks = _teams.Count;
        while (remainingChecks > 0 && _teams[ActiveTeamIdx].GetLeftMemberCount() < 1)
        {
            _turn++;
            remainingChecks--;
        }

        if (remainingChecks == 0 && _teams[ActiveTeamIdx].GetLeftMemberCount() < 1)
        {
            return;
        }

        StartTurnClientRpc(_turn);
    }

    [ClientRpc]
    private void StartTurnClientRpc(int turn)
    {
        _turn = turn;
        if (_teams.Count == 0)
        {
            return;
        }

        _teams[ActiveTeamIdx].StartTurn();
    }

    public IDisposable Subscribe(IObserver<int> observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }

        return new Unsubscriber<int>(_observers, observer);
    }

    private void TeamRetire(int retireTeamIdx)
    {
        foreach (IObserver<int> target in _observers)
        {
            target.OnNext(retireTeamIdx);
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        NewSystemJoinServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void NewSystemJoinServerRpc()
    {
        SyncTurnInforClientRpc(_turn);
    }

    [ClientRpc]
    private void SyncTurnInforClientRpc(int turn)
    {
        _turn = turn;
    }

}
