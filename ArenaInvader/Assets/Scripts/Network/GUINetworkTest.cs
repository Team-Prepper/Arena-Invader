using EasyH.Unity.UI;

public class GUINetworkTest : GUIFullScreen {
    public void StartHost() => NetManager.Instance.System.StartHost();
    public void StartClient() => NetManager.Instance.System.StartClient();
}