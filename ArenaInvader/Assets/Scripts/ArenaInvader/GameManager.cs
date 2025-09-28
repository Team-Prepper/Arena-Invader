using System;
using EasyH.Unity;

public class GameManager : MonoSingleton<GameManager> {

    public IMatchInfor MatchInfor { get; set; }
    public IPlayground Playground { get; set; } 

    public Action OnMatchInforChanged { get; set; }

    protected override void OnCreate()
    {
        base.OnCreate();

        Playground = new Playground();
        MatchInfor = new MatchInfor(2, "Map/DefaultMap", "DartDice");

    }

}