using Fantasy.Entitas;
using Fantasy.Network;
using Fantasy.Serialize;

namespace Fantasy
{
    public class SerializerComponent : Entity
    {
        public ISerialize Serialize;
        public readonly MemoryStreamBufferPool BufferPool = new MemoryStreamBufferPool();
    }
}