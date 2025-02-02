using Unity.Collections;
using Unity.Netcode;

public struct UNetString : INetworkSerializable {

    public FixedString32Bytes Value;

    public UNetString(string value) {
        Value = value;
    }
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Value);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}