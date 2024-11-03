using EHTool;

public class GameManager : Singleton<GameManager> {
    public Playground Playground { get; set; } = new Playground();
    
}