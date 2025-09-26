using UnityEngine;
using EasyH.Unity;

public class NetManager : MonoSingleton<NetManager>
{
    public INetwork System { get; set; }
}
