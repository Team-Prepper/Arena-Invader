using Unity.Netcode;
using UnityEngine;

public class UNetTest : NetworkBehaviour {


    private void Update()
    {
        TestIntClientRpc(1);
        TestIntServerRpc(3);
        TestClientRpc("helloworld");
        TestServerRpc("helloserver");

        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("H");

        }
        /*
            if (Input.GetKeyDown(KeyCode.H))
        {
        }*/
    }

    [ClientRpc]
    public void TestIntClientRpc(int i)
    {
        Debug.Log(i);
    }

    [ServerRpc]
    public void TestIntServerRpc(int i)
    {
        Debug.Log(i);

    }

    [ClientRpc]
    public void TestClientRpc(string i)
    {
        Debug.Log(i);
    }

    [ServerRpc]
    public void TestServerRpc(string i)
    {
        Debug.Log(i);

    }
}