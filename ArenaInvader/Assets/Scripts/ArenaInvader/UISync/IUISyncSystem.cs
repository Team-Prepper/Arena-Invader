public interface IUISyncSystem
{
    public bool IsSynchronized { get; }
    public void MarkDirty();
    public void Reset();
}
