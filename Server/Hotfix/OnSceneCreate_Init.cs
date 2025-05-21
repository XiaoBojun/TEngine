using Fantasy.Async;
using Fantasy.Event;

namespace Fantasy;

public sealed class OnSceneCreate_Init : AsyncEventSystem<OnCreateScene>
{
    protected override async FTask Handler(OnCreateScene self)
    {
        var scene = self.Scene;

        switch (scene.SceneType)
        {
            case SceneType.Gate:
            {
                // GateUnit管理组件。
                scene.AddComponent<GateUnitManageComponent>();
                break;
            }
            case SceneType.Chat:
            {
                // 序列化组件。
                scene.AddComponent<SerializerComponent>().Initialize();
                // ChatUnit管理组件。
                scene.AddComponent<ChatUnitManageComponent>();
                // 聊天频道中控中心组件。
                scene.AddComponent<ChatChannelCenterComponent>();
                break;
            }
            // case SceneType.Authentication:
            // {
            //     // 用于鉴权服务器注册和登录相关逻辑的组件
            //     //scene.AddComponent<AuthenticationComponent>().UpdatePosition();
            //     // 用于颁发ToKen证书相关的逻辑。
            //     //scene.AddComponent<AuthenticationJwtComponent>();
            //     
            //     //     // 序列化组件。
            //     //     scene.AddComponent<SerializerComponent>().Initialize();
            //     //     // ChatUnit管理组件。
            //     //     scene.AddComponent<ChatUnitManageComponent>();
            //     //     // 聊天频道中控中心组件。
            //     //     scene.AddComponent<ChatChannelCenterComponent>();
            //     break;
            // }
            // case SceneType.Gate:
            // {
            //     //     // GateUnit管理组件。
            //     //     scene.AddComponent<GateUnitManageComponent>();
            //     // 用于验证JWT是否合法的组件
            //     //scene.AddComponent<GateJWTComponent>();
            //     // 用于管理GameAccount的组件
            //     //scene.AddComponent<GameAccountManageComponent>();
            //     break;
            // }
        }
        await FTask.CompletedTask;
    }
}