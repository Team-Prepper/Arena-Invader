public interface INetworkMatchInfo
{
    public int GetPlayerIdx(ulong clientId);
    public ulong GetClientId(int playerIdx);
}
