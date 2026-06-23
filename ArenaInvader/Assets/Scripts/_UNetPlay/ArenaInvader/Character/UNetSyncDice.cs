using System;
using Unity.Netcode;

public class UNetSyncDice : NetworkBehaviour, IOpenDice {

    private NetworkSyncUIConnector<GUIDice, float> _diceSync;

    public void Initial(IPlayableCharacter cc)
    {
        _diceSync = new NetworkSyncUIConnector<GUIDice, float>(
            GameManager.Instance.MatchInfo.MatchDice);
        
    }

    public GUIDice OpenDice(Action<int> callback)
    {

        GUIDice dice = _diceSync.ControlClientOpen();

        dice.SetCallback((value) => {
            callback?.Invoke(value);
            CloseDiceServerRpc();
        });
        dice.NetworkModifiedMethodSet(DiceChangeServerRpc);

        int seed = DateTime.Now.Millisecond;
        dice.SetSeed(seed);

        OpenDiceServerRpc(seed);

        return dice;

    }

    [ServerRpc(RequireOwnership = false)]
    void DiceChangeServerRpc(float value)
        => DiceChangeClientRpc(value);

    [ClientRpc(RequireOwnership = false)]
    void DiceChangeClientRpc(float value) {
        _diceSync.SetModified(value);
    }

    [ServerRpc(RequireOwnership = false)]
    void OpenDiceServerRpc(int seed)
        => OpenDiceClientRpc(seed);

    [ClientRpc(RequireOwnership = false)]
    void OpenDiceClientRpc(int seed) {
        
        _diceSync.ClientOpen((ui) => {
            ui.SetSeed(seed);
        });
        
    }

    [ServerRpc(RequireOwnership = false)]
    void CloseDiceServerRpc()
        => CloseDiceClientRpc();

    [ClientRpc(RequireOwnership = false)]
    void CloseDiceClientRpc()
        => _diceSync.Close();

}
