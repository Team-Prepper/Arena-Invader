using System;
using Unity.Netcode;

public class UNetSyncDice : NetworkBehaviour {

    NetworkSyncUIConnector<GUIDice, int> _diceSync;
    ICharacterController _cc;

    public void Initial(ICharacterController cc)
    {
        _cc = cc;
        _diceSync = new NetworkSyncUIConnector<GUIDice, int>(
            GameManager.Instance.MatchInfor.MatchDice);

    }

    public GUIDice OpenDice(Action<int> callback)
    {

        GUIDice dice = _diceSync.ControlClientOpen();

        dice.SetCallback((value) => {
            callback?.Invoke(value);
            CloseDiceServerRpc();
        });

        OpenDiceServerRpc();

        return dice;
    }

    [ServerRpc(RequireOwnership = false)]
    void OpenDiceServerRpc() => OpenDiceClientRpc();

    [ClientRpc(RequireOwnership = false)]
    void OpenDiceClientRpc() => _diceSync.ClientOpen();


    [ServerRpc(RequireOwnership = false)]
    void CloseDiceServerRpc() => CloseDiceClientRpc();

    [ClientRpc(RequireOwnership = false)]
    void CloseDiceClientRpc() => _diceSync.Close();


}