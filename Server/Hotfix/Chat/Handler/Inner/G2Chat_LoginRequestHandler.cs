using Fantasy.Async;
using Fantasy.Entitas;
using Fantasy.Network.Interface;

namespace Fantasy;

public sealed class G2Chat_LoginRequestHandler : RouteRPC<Scene, G2Chat_LoginRequest, Chat2G_LoginResponse>
{
    protected override async FTask Run(Scene scene, G2Chat_LoginRequest request, Chat2G_LoginResponse response, Action reply)
    {
        var chatUnit = scene.GetComponent<ChatUnitManageComponent>().Add(request.UnitId, request.UserName, request.GateRouteId);
        response.ChatRouteId = chatUnit.RuntimeId;
        // 这里模拟创建一个频道用于测试用
        var chatChannelCenterComponent = scene.GetComponent<ChatChannelCenterComponent>();
        var chatChannelComponent = chatChannelCenterComponent.Apply(1);
        // 加入到聊天频道
        chatChannelComponent.JoinChannel(request.UnitId);
        await FTask.CompletedTask;
    }
}