using System;
using Unity.Netcode;

public class UNetSyncBattle : NetworkBehaviour, IOpenBattle {

    private NetworkSyncUIConnector<GUIBattle, int> _battleSync;

    public void Initial(IPlayableCharacter cc)
    {
        _battleSync = new NetworkSyncUIConnector<GUIBattle, int>("Battle");

    }

    public GUIBattle OpenBattle(int attacker, int target, Action callback)
    {
        GUIBattle battle = _battleSync.ControlClientOpen();

        battle.BattleSet(attacker, target);
        battle.StartBattle(callback);
        battle.NetworkModifiedMethodSet(BattleSequenceChangeServerRpc);

        battle.SetCloseMethod(() =>
        {
            CloseBattleServerRpc();
        });
        OpenBattleServerRpc(attacker, target);

        return battle;
    }

    [ServerRpc(RequireOwnership = false)]
    void BattleSequenceChangeServerRpc(int value) => BattleSequenceChangeClientRpc(value);

    [ClientRpc]
    void BattleSequenceChangeClientRpc(int value)
    {
        _battleSync.SetModified(value);
    }

    [ServerRpc(RequireOwnership = false)]
    private void OpenBattleServerRpc(int attacker, int target)
        => OpenBattleClientRpc(attacker, target);

    [ClientRpc(RequireOwnership = false)]
    private void OpenBattleClientRpc(int attacker, int target) {
        _battleSync.ClientOpen((ui) => {
            ui.BattleSet(attacker, target);
        });
    }

    [ServerRpc(RequireOwnership = false)]
    private void CloseBattleServerRpc() => CloseBattleClientRpc();

    [ClientRpc(RequireOwnership = false)]
    private void CloseBattleClientRpc() => _battleSync.Close();


}