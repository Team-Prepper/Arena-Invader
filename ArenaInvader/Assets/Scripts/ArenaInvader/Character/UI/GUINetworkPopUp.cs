using System;
using EasyH.Unity.UI;

public class GUINetworkPopUp<T> : GUIPopUp
{
    protected bool IsControlled { get; private set; } = true;
    
    private Action<T> _networkModifedAction;
    private Action _closeAction;

    public void NetworkModify(T value) {
        _networkModifedAction?.Invoke(value);
    }

    public void SetIsNotControlled() {
        IsControlled = false;
    }

    public void TryClose() {
        if (!IsControlled) return;

        _closeAction?.Invoke();
        Close();
    }

    public override void SetOff()
    {
        if (IsControlled) {
            base.SetOff();
            return;
        }
        Close();
    }

    public override void Close()
    {
        base.Close();
    }

    public void SetCloseMethod(Action closeAction) { 
        _closeAction = closeAction;
    }

    public void AddCloseMethod(Action closeAction) {
        _closeAction += closeAction;
    }

    public void NetworkModifiedMethodSet(Action<T> action) {
        _networkModifedAction = action;
    }

    public virtual void NetworkModifiedEvent(T value) { 
        
    }

}