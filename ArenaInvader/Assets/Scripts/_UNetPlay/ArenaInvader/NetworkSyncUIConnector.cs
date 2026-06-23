using System;
using UnityEngine;
using EasyH.Unity.UI;

public class NetworkSyncUIConnector<T, K>
    where T : GUINetworkPopUp<K> {

    T _gui;

    string _key = string.Empty;

    public NetworkSyncUIConnector() {

    }

    public void SetModified(K value) {
        if (_gui == null) return;
        _gui.NetworkModifiedEvent(value);
    }

    public NetworkSyncUIConnector(string defaultKey)
    {
        _key = defaultKey;
    }

    public bool IsOpen => _gui != null;

    public T CurrentGUI => _gui;

    public T ControlClientOpen() {
        _gui = UIManager.Instance.OpenGUI<T>(_key);
        return _gui;
    }

    public void Close()
    {
        if (_gui == null) return;
        _gui.Close();
        _gui = null;
    }

    public void ClientOpen(Action<T> method = null) {
        if (_gui != null) return;

        ControlClientOpen();

        _gui.SetIsNotControlled();
        method?.Invoke(_gui);
    }

}
