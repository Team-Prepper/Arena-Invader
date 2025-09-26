using Unity.Netcode;

public class UNetSyncMovePawn : NetworkBehaviour, IOpenSelectPawn {

    private NetworkSyncUIConnector<GUISelectMovePawn, int> _selectMovePawnSync;
    private IPlayableCharacter _cc;

    public void Initial(IPlayableCharacter cc)
    {
        _cc = cc;
        _selectMovePawnSync =
            new NetworkSyncUIConnector<GUISelectMovePawn, int>("SelectMovePawn");
    }

    public GUISelectMovePawn OpenSelectMovePawn(int value, int addedValue)
    {
        GUISelectMovePawn movePawn = _selectMovePawnSync.ControlClientOpen();

        _cc.PawnOwner.OnPawnChoose();

        movePawn.SetPlayer(_cc, value, addedValue);

        movePawn.SetCloseMethod(() =>
        {
            CloseSelectMovePawnServerRpc();
        });

        movePawn.NetworkModifiedMethodSet(PawnMoveValueChangeServerRpc);

        OpenSelectMovePawnServerRpc(value, addedValue);

        return movePawn;
    }

    [ServerRpc(RequireOwnership = false)]
    void PawnMoveValueChangeServerRpc(int value)
        => PawnMoveValueChangeClientRpc(value);

    [ClientRpc]
    void PawnMoveValueChangeClientRpc(int value)
    {
        _selectMovePawnSync.SetModified(value);
    }

    [ServerRpc(RequireOwnership = false)]
    void OpenSelectMovePawnServerRpc(int value, int addedValue)
        => OpenSelectMovePawnClientRpc(value, addedValue);

    [ClientRpc]
    void OpenSelectMovePawnClientRpc(int value, int addedValue)
    {
        _selectMovePawnSync.ClientOpen((ui) => {
            _cc.PawnOwner.OnPawnChoose();
            ui.SetPlayer(_cc, value, addedValue);
            ui.SetCloseMethod(_cc.PawnOwner.OffPawnChoose);
        });
    }

    [ServerRpc(RequireOwnership = false)]
    void CloseSelectMovePawnServerRpc() => CloseSelectMovePawnClientRpc();

    [ClientRpc]
    void CloseSelectMovePawnClientRpc() => _selectMovePawnSync.Close();

}