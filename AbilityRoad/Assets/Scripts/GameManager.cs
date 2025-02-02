using EHTool;
using System;
using Unity.Networking.Transport;

public class GameManager : MonoSingleton<GameManager> {

    public IMatchInfor MatchInfor { get; set; }
    public IPlayground Playground { get; set; } 

    public INetwork Network { get; private set; }

    public Action OnMatchInforChanged { get; set; }

    protected override void OnCreate()
    {
        base.OnCreate();

        Playground = new LocalPlayground();
        MatchInfor = new LocalMatchInfor(2, "Map/DefaultMap", "DartDice");

        Network = gameObject.AddComponent<UNetNetwork>();
    }

}