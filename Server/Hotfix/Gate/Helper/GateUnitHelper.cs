using Fantasy.Async;
using Fantasy.Network;
// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

#pragma warning disable CS8602 // Dereference of a possibly null reference.
namespace Fantasy;

public static class GateUnitHelper
{
    /// <summary>
    /// GateUnit上线
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="userName"></param>
    /// <param name="session"></param>
    /// <returns></returns>
    public static GateUnit Online(Scene scene, string userName, Session session)
    {
        // 增加一个GateUnit到缓存中、如果缓存中已经存在直接返回缓存中的实体数据
        return scene.GetComponent<GateUnitManageComponent>().Add(userName, session);
    }

    /// <summary>
    /// GateUnit下线
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="gateUnitId"></param>
    public static void Offline(Scene scene, long gateUnitId)
    {
        if (!scene.GetComponent<GateUnitManageComponent>().TryGet(gateUnitId, out var gateUnit))
        {
            return;
        }

        gateUnit.GetOrAddComponent<EntityTimeoutComponent>().SetTimeout(5000);
    }
    
    /// <summary>
    /// 缓存中获取一个GateUnit
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="userName"></param>
    /// <returns></returns>
    public static GateUnit? Get(Scene scene, string userName)
    {
        return scene.GetComponent<GateUnitManageComponent>().Get(userName);
    }
    
    /// <summary>
    /// 缓存中获取一个GateUnit
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="gateUnitId"></param>
    /// <returns></returns>
    public static GateUnit? Get(Scene scene, long gateUnitId)
    {
        return scene.GetComponent<GateUnitManageComponent>().Get(gateUnitId);
    }
    
    /// <summary>
    /// 尝试获取一个GateUnit
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="userName"></param>
    /// <param name="gateUnit"></param>
    /// <returns></returns>
    public static bool TryGet(Scene scene, string userName, out GateUnit? gateUnit)
    {
        return scene.GetComponent<GateUnitManageComponent>().TryGet(userName, out gateUnit);
    }

    /// <summary>
    /// 尝试获取一个GateUnit
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="gateUnitId"></param>
    /// <param name="gateUnit"></param>
    /// <returns></returns>
    public static bool TryGet(Scene scene, long gateUnitId, out GateUnit? gateUnit)
    {
        return scene.GetComponent<GateUnitManageComponent>().TryGet(gateUnitId, out gateUnit);
    }

    /// <summary>
    /// 在缓存中移除一个GateUnit
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="userName"></param>
    /// <param name="isDispose"></param>
    public static FTask Remove(Scene scene, string userName, bool isDispose = true)
    {
        return scene.GetComponent<GateUnitManageComponent>().Remove(userName, isDispose);
    }

    /// <summary>
    /// 在缓存中移除一个GateUnit
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="gateUnitId"></param>
    /// <param name="isDispose"></param>
    public static FTask Remove(Scene scene, long gateUnitId, bool isDispose = true)
    {
        return scene.GetComponent<GateUnitManageComponent>().Remove(gateUnitId, isDispose);
    }
}