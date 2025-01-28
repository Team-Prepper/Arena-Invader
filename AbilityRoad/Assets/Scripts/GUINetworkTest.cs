using EHTool.UIKit;

public class GUINetworkTest : GUIFullScreen {
    public void StartHost() => GameManager.Instance.Network.StartHost();
    public void StartClient() => GameManager.Instance.Network.StartClient();
}