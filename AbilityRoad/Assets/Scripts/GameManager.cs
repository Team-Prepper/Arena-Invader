using EHTool;
using Unity.Networking.Transport;

public class GameManager : MonoSingleton<GameManager> {
    public IPlayground Playground { get; set; } = new LocalPlayground();

    public INetwork Network { get; private set; }

    protected override void OnCreate()
    {
        base.OnCreate();

        Network = gameObject.AddComponent<UNetNetwork>();
    }

}