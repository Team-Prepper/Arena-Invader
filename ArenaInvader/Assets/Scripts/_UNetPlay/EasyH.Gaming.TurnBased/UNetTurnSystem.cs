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

    public int TurnSpend => _turn / _teams.Count;
    public int ActiveTeamIdx => _turn % _teams.Count;

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
        _teams[m.TeamIdx].RemoveMember(m);

        if (!IsOwner) return;
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
        if (!IsOwner) return;
        _turn = 0;
        StartTurnClientRpc(_turn);
    }

    public void TurnEnd()
    {
        if (!IsOwner) return;
        if (!_condition()) return;

        _turn++;

        while (_teams[ActiveTeamIdx].GetLeftMemberCount() < 1)
        {
            _turn++;
        }

        StartTurnClientRpc(_turn);
    }

    [ClientRpc]
    private void StartTurnClientRpc(int turn)
    {
        _turn = turn;
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
