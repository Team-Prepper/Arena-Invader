using Unity.Netcode;
using UnityEngine;

public class UNetCharacterController : LocalCharacterController
{
    [ClientRpc]
    public void TestClientRpc(string s) {
        Debug.Log(s);
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.H))
        {
            TestClientRpc("ee");
        }
    }
}
