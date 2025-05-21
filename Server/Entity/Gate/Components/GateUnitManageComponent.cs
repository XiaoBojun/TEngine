using Fantasy.Entitas;

namespace Fantasy;

public sealed class GateUnitManageComponent : Entity
{
    public readonly Dictionary<long, GateUnit> Units = new Dictionary<long, GateUnit>();
    public readonly Dictionary<string, GateUnit> UnitsByUserName = new Dictionary<string, GateUnit>();
}