using Fantasy.Serialize;
#pragma warning disable CS8604 // Possible null reference argument.

namespace Fantasy;

public static class SerializerComponentSystem
{
    public static void Initialize(this SerializerComponent self)
    {
        self.Serialize = SerializerManager.GetSerializer(FantasySerializerType.ProtoBuf);
    }
    
    public static byte[] Serialize<T>(this SerializerComponent self, T @object)
    {
        using var memoryStreamBuffer = self.BufferPool.RentMemoryStream(MemoryStreamBufferSource.None);
        self.Serialize.Serialize(typeof(T), @object, memoryStreamBuffer);
        return memoryStreamBuffer.ToArray();
    }

    public static T Deserialize<T>(this SerializerComponent self, byte[] bytes)
    {
        return self.Serialize.Deserialize<T>(bytes);
    }
}