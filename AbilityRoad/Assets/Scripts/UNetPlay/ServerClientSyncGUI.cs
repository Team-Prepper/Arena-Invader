using EHTool.UIKit;

public class ServerClientSyncGUI<T> where T : GUIWindow {

    T _gui;

    string _key = string.Empty;

    public ServerClientSyncGUI() {

    }

    public ServerClientSyncGUI(string defaultKey)
    {
        _key = defaultKey;
    }

    public T Client¾îÂ¼±¸() {
        _gui = UIManager.Instance.OpenGUI<T>(_key);
        return _gui;
    }

    public void Close()
    {
        _gui?.Close();
        _gui = null;
    }

    public void ClientOpen() {
        if (_gui != null) return;
        Client¾îÂ¼±¸();
    }

}