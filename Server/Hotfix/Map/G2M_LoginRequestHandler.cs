using Fantasy.Async;
using Fantasy.Network.Interface;

namespace Fantasy.Map;

public class G2M_LoginRequestHandler : RouteRPC<Scene,G2M_LoginRequest, M2G_LoginResponse>
{
    protected override async FTask Run(Scene scene, G2M_LoginRequest request, M2G_LoginResponse response, Action reply)
    {
        var chatInfoTree = ChatTreeFactory.Broadcast(scene).AddendTextNode("您杀死了奥格瑞玛！");
        scene.TimerComponent.Net.RepeatedTimer(2000, () =>
        {
            ChatHelper.SendChatMessage(scene, request.ChatUnitRouteId, chatInfoTree);
        });
        await FTask.CompletedTask;
    }
}