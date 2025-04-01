public interface INetwork {
    
    public int Id { get; }
    public int GetIdx(ulong clientId);

    public void StartHost();
    public void StartClient();
    public void Disconnect();

}