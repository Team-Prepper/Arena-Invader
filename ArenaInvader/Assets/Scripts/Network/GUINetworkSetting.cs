using EasyH.Unity.UI;

public class GUINetworkSetting : GUIFullScreen
{

    public override void Open()
    {
        base.Open();
        UNetNetwork.OnNetwork();
    }
    public void StartHost()
    {
        NetManager.Instance.System.StartHost();
    }

    public void StartClient() {
        NetManager.Instance.System.StartClient();
    }

}
