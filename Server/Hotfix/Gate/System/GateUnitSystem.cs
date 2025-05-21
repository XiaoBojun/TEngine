using Fantasy.Entitas.Interface;
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

namespace Fantasy;

public sealed class GateUnitDestroySystem : DestroySystem<GateUnit>
{
    protected override void Destroy(GateUnit self)
    {
        // 移除缓存中的GateUnit
        // 这里的销毁，只能是通过EntityTimeoutComponent超时来触发的销毁，不能通过直接调用Dispose来触发的销毁
        GateUnitHelper.Remove(self.Scene, self.Id, false).Coroutine();
    }
}