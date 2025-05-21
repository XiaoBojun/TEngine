using Fantasy.Async;
using Fantasy.Network.Interface;

namespace Fantasy;

public sealed class G2Chat_OfflineRequestHandler : RouteRPC<ChatUnit, G2Chat_OfflineRequest, Chat2G_OfflineResponse>
{
    protected override async FTask Run(ChatUnit chatUnit, G2Chat_OfflineRequest request, Chat2G_OfflineResponse response, Action reply)
    {
        await FTask.CompletedTask;
        chatUnit.Scene.GetComponent<ChatUnitManageComponent>().Remove(chatUnit.Id);
    }
}