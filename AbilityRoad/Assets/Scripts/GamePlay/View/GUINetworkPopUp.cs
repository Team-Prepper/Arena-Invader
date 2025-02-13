using EHTool.UIKit;
using System;

public class GUINetworkPopUp<T> : GUIPopUp
{
    protected bool IsControlled { get; private set; } = true;
    
    private Action<T> _networkModifedAction;
    private Action _closeAction;

    protected void NetworkModify(T value) {
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

    public void SetCloseMethod(Action closeAction) { 
        _closeAction = closeAction;
    }

    public void NetworkModifiedMethodSet(Action<T> action) {
        _networkModifedAction = action;
    }

    public virtual void NetworkModifiedEvent(T value) { 
        
    }

}