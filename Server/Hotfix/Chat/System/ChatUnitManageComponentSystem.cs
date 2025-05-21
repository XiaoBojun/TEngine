using Fantasy.Entitas;
using Fantasy.Entitas.Interface;
#pragma warning disable CS8601 // Possible null reference assignment.

namespace Fantasy;

public sealed class ChatUnitManageComponentDestroySystem : DestroySystem<ChatUnitManageComponent>
{
    protected override void Destroy(ChatUnitManageComponent self)
    {
        foreach (var chatUnit in self.Units.Values.ToArray())
        {
            chatUnit.Dispose();
        }
        
        self.Units.Clear();
    }
}

public static class ChatUnitManageComponentSystem
{
    public static ChatUnit Add(this ChatUnitManageComponent self, long unitId, string userName, long gateRouteId)
    {
        if (!self.Units.TryGetValue(unitId, out var chatUnit))
        {
            chatUnit = Entity.Create<ChatUnit>(self.Scene, unitId, true, true);
            self.Units.Add(unitId, chatUnit);
            Log.Debug($"Add ChatUnit Count: {self.Units.Count}  UnitId: {unitId}  UserName: {userName}  GateRouteId: {gateRouteId}");
        }
        else
        {
            Log.Debug($"ChatUnit: {chatUnit.UserName}({chatUnit.GateRouteId})");
        }

        chatUnit.UserName = userName;
        chatUnit.GateRouteId = gateRouteId;
        return chatUnit;
    }
    
    public static ChatUnit? Get(this ChatUnitManageComponent self, long unitId)
    {
        return self.Units.GetValueOrDefault(unitId);
    }

    public static bool TryGet(this ChatUnitManageComponent self, long unitId, out ChatUnit chatUnit)
    {
        return self.Units.TryGetValue(unitId, out chatUnit);
    }

    public static void Remove(this ChatUnitManageComponent self, long unitId, bool isDispose = true)
    {
        // 由于退出频道的时候，也会检查该玩家是否在ChatUnitManageComponent中，所以这里不做移除操作。
        if (!self.Units.TryGetValue(unitId, out var chatUnit))
        {
            return;
        }
        
        if (isDispose)
        {
            chatUnit.Dispose();
        }
        
        // 因为玩家已经执行了退出频道的操作了，所以要清除一下这个数据。
        self.Units.Remove(unitId);
        Log.Debug($"Remove ChatUnit: {chatUnit.UserName}({chatUnit.GateRouteId}) Count: {self.Units.Count}");
    }
}