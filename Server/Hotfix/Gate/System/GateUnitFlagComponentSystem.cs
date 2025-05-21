using Fantasy.Entitas.Interface;

namespace Fantasy;

public sealed class GateUnitFlagComponentDestroySystem : DestroySystem<GateUnitFlagComponent>
{
    protected override void Destroy(GateUnitFlagComponent self)
    {
        var selfGateUnitId = self.GateUnitId;
        // 执行下线操作
        GateUnitHelper.Offline(self.Scene, selfGateUnitId);
        // 清理垃圾数据
        self.GateUnitId = 0;
    }
}