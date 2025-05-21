// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

using Fantasy.Entitas.Interface;

namespace Fantasy;
public sealed class EntityTimeoutComponentDestroySystem : DestroySystem<EntityTimeoutComponent>
{
    protected override void Destroy(EntityTimeoutComponent self)
    {
        self.CancelTimeout();
    }
}

public static class EntityTimeoutComponentSystem
{
    // 这个组件会挂载到目标组件上
    // 当这个组件的超时时间到了，会自动销毁这个组件的父亲
    
    public static void SetTimeout(this EntityTimeoutComponent self, int time)
    {
        var selfParent = self.Parent;
        if (selfParent == null)
        {
            Log.Error("EntityTimeoutComponent's parent is null.");
            return;
        }

        var selfParentRunTimeId = selfParent.RuntimeId;
        self.TimerId = self.Scene.TimerComponent.Net.OnceTimer(time, () =>
        {
            if (selfParent.RuntimeId != selfParentRunTimeId)
            {
                return;
            }

            self.TimerId = 0;
            selfParent.Dispose();
        });
    }
    
    public static void CancelTimeout(this EntityTimeoutComponent self)
    {
        if (self.TimerId == 0)
        {
            return;
        }

        self.Scene.TimerComponent.Net.Remove(ref self.TimerId);
    }
}