using System.Collections;
using System.Collections.Generic;
using EHTool;
using UnityEngine;

public class PopUpManager : MonoSingleton<PopUpManager>
{
    [SerializeField] GameObject _popUpText;
    [SerializeField] Transform _popUpParent;
    
    public void ShowPopUp(string message)
    {
        GameObject popUp = Instantiate(_popUpText, _popUpParent);
        popUp.GetComponent<PopUpText>().SetText(message);
    }
}
