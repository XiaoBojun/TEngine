using Fantasy;
using Fantasy.Async;
using Fantasy.Event;
using Fantasy.Gate;

namespace Hotfix;

public class OnCreateScene_AsyncEvent : AsyncEventSystem<OnCreateScene>
{
    protected override async FTask Handler(OnCreateScene self)
    {
        var scene = self.Scene;
        switch (scene.SceneType)
        {
            case SceneType.Gate:
            {
                self.Scene.AddComponent<GateComponent>();
                break;
            }
        }
        await FTask.CompletedTask;
    }
}