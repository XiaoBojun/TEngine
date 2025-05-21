using Fantasy.Entitas;
using Fantasy.Network;

namespace Fantasy;

public sealed class GateUnit : Entity
{
    public EntityReference<Session> Session;
    public string UserName { get; set; }
    public readonly Dictionary<int, long> Routes = new Dictionary<int, long>();
}