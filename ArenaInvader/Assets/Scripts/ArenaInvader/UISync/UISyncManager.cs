using EasyH;

public class UISyncManager : Singleton<UISyncManager>
{
    public IUISyncSystem System { get; set; }

    protected override void OnCreate()
    {
        System = new UISyncSystem();
    }
}
