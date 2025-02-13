public interface INetwork {
    int Id { get; }

    public void StartHost();
    public void StartClient();
    public void Disconnect();

}