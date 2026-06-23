public class UISyncSystem : IUISyncSystem
{
    public bool IsSynchronized { get; private set; } = true;

    public void MarkDirty()
    {
        IsSynchronized = false;
    }

    public void Reset()
    {
        IsSynchronized = true;
    }
}
