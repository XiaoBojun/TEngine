using Fantasy.Async;
using Fantasy.Entitas;
using Fantasy.Entitas.Interface;
using Fantasy.Network;
// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

namespace Fantasy;

public sealed class GateUnitManageComponentDestroySystem : DestroySystem<GateUnitManageComponent>
{
    protected override void Destroy(GateUnitManageComponent self)
    {
        foreach (var gateUnit in self.Units.Values.ToArray())
        {
            gateUnit.Dispose();
        }
        
        self.Units.Clear();
        self.UnitsByUserName.Clear();
    }
}

public static class GateUnitManageComponentSystem
{
    public static GateUnit Add(this GateUnitManageComponent self, string userName, Session session)
    {
        if (self.UnitsByUserName.TryGetValue(userName, out var gateUnit))
        {
            gateUnit.Session = session;
            // 如果缓存中已经存在了该名字，那就直接从缓存中返回就可以了
            Log.Debug($"在缓存中获取的数据 userName:{userName}");
            return gateUnit;
        }

        // 创建一个新的实体
        gateUnit = Entity.Create<GateUnit>(self.Scene, true, true);
        gateUnit.UserName = userName;
        gateUnit.Session = session;
        // 添加到缓存中
        self.Units.Add(gateUnit.Id, gateUnit);
        self.UnitsByUserName.Add(userName, gateUnit);

        Log.Debug($"新创建的数据 userName:{userName}");
        return gateUnit;
    }

    public static GateUnit? Get(this GateUnitManageComponent self, string userName)
    {
        return self.UnitsByUserName.GetValueOrDefault(userName);
    }
    
    public static GateUnit? Get(this GateUnitManageComponent self, long gateUnitId)
    {
        return self.Units.GetValueOrDefault(gateUnitId);
    }
    
    public static bool TryGet(this GateUnitManageComponent self, string userName, out GateUnit? gateUnit)
    {
        return self.UnitsByUserName.TryGetValue(userName, out gateUnit);
    }

    public static bool TryGet(this GateUnitManageComponent self, long gateUnitId, out GateUnit? gateUnit)
    {
        return self.Units.TryGetValue(gateUnitId, out gateUnit);
    }

    public static async FTask Remove(this GateUnitManageComponent self, string userName, bool isDispose = true)
    {
        if (!self.UnitsByUserName.TryGetValue(userName, out var gateUnit))
        {
            return;
        }
        
        // 通知其他服务器下线
        var result = await GateLoginHelper.Offline(gateUnit);
        if (result != 0)
        {
            Log.Error($"通知其他服务器下线失败，错误码：{result}");
            return;
        }
        // 如果其他服务器都已经下线了，那就直接移除本地数据
        self.Units.Remove(gateUnit.Id);
        self.UnitsByUserName.Remove(userName);
        
        if (isDispose)
        {
            gateUnit.Dispose();
        }

        gateUnit.Session = null;
        gateUnit.UserName = null;
    }
    
    public static async FTask Remove(this GateUnitManageComponent self, long gateUnitId, bool isDispose = true)
    {
        if (!self.Units.TryGetValue(gateUnitId, out var gateUnit))
        {
            return;
        }
        // 通知其他服务器下线
        var result = await GateLoginHelper.Offline(gateUnit);
        if (result != 0)
        {
            Log.Error($"通知其他服务器下线失败，错误码：{result}");
            return;
        }
        // 如果其他服务器都已经下线了，那就直接移除本地数据
        self.Units.Remove(gateUnitId);
        self.UnitsByUserName.Remove(gateUnit.UserName);
        
        if (isDispose)
        {
            gateUnit.Dispose();
        }

        gateUnit.Session = null;
        gateUnit.UserName = null;
    }

    public static IEnumerable<Session> ForEachUnitSession(this GateUnitManageComponent self)
    {
        foreach (var (_, gateUnit) in self.Units)
        {
            Session gateUnitSession = gateUnit.Session;

            if (gateUnitSession == null)
            {
                continue;
            }

            yield return gateUnitSession;
        }
    }
}