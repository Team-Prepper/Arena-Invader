using System;
using EasyH.Unity;

public class GameManager : MonoSingleton<GameManager> {

    public IMatchInfo MatchInfo { get; set; }
    public IPlayground Playground { get; set; } 

    public Action OnMatchInfoChanged { get; set; }

    protected override void OnCreate()
    {
        base.OnCreate();

        Playground = new Playground();
        MatchInfo = null;

    }

}
