using Fantasy.Entitas;

namespace Fantasy;

public sealed class ChatUnitManageComponent : Entity
{
    public readonly Dictionary<long, ChatUnit> Units = new();
}