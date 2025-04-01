using EHTool.UIKit;

public class GUINetworkSetting : GUIFullScreen
{
    public void StartHost() {
        GameManager.Instance.Network.StartHost();
    }

    public void StartClient() {
        GameManager.Instance.Network.StartClient();
    }

}
