using Unity.Netcode;

public class UNetSyncMovePawn : NetworkBehaviour {

    NetworkSyncUIConnector<GUISelectMovePawn, int> _selectMovePawnSync;
    ICharacterController _cc;

    public void Initial(ICharacterController cc)
    {
        _cc = cc;
        _selectMovePawnSync =
            new NetworkSyncUIConnector<GUISelectMovePawn, int>("SelectMovePawn");
    }

    public GUISelectMovePawn OpenSelectMovePawn(int value)
    {
        GUISelectMovePawn movePawn = _selectMovePawnSync.ControlClientOpen();

        _cc.Target.OnPawnChoose();

        movePawn.SetPlayer(_cc, value);

        movePawn.SetCloseMethod(() =>
        {
            CloseSelectMovePawnServerRpc();
        });

        movePawn.NetworkModifiedMethodSet(PawnMoveValueChangeServerRpc);

        OpenSelectMovePawnServerRpc(value);

        return movePawn;
    }

    [ServerRpc(RequireOwnership = false)]
    void PawnMoveValueChangeServerRpc(int value) => PawnMoveValueChangeClientRpc(value);

    [ClientRpc]
    void PawnMoveValueChangeClientRpc(int value)
    {
        _selectMovePawnSync.SetModified(value);
    }

    [ServerRpc(RequireOwnership = false)]
    void OpenSelectMovePawnServerRpc(int value) => OpenSelectMovePawnClientRpc(value);

    [ClientRpc]
    void OpenSelectMovePawnClientRpc(int value)
    {
        _selectMovePawnSync.ClientOpen((ui) => {
            _cc.Target.OnPawnChoose();
            ui.SetPlayer(_cc, value);
            ui.SetCloseMethod(_cc.Target.OffPawnChoose);
        });
    }

    [ServerRpc(RequireOwnership = false)]
    void CloseSelectMovePawnServerRpc() => CloseSelectMovePawnClientRpc();

    [ClientRpc]
    void CloseSelectMovePawnClientRpc() => _selectMovePawnSync.Close();

}