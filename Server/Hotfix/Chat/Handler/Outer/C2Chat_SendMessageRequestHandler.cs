using Fantasy.Async;
using Fantasy.Network.Interface;

namespace Fantasy;

public sealed class C2Chat_SendMessageRequestHandler : RouteRPC<ChatUnit, C2Chat_SendMessageRequest, Chat2C_SendMessageResponse>
{
    protected override async FTask Run(ChatUnit chatUnit, C2Chat_SendMessageRequest request, Chat2C_SendMessageResponse response, Action reply)
    {
        response.ErrorCode = ChatSceneHelper.Distribution(chatUnit, request.ChatInfoTree);
        await FTask.CompletedTask;
    }
}