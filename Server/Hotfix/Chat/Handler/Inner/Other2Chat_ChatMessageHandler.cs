using Fantasy.Async;
using Fantasy.Network.Interface;

namespace Fantasy;

public sealed class Other2Chat_ChatMessageHandler : Route<ChatUnit, Other2Chat_ChatMessage>
{
    protected override async FTask Run(ChatUnit chatUnit, Other2Chat_ChatMessage message)
    {
        var result = ChatSceneHelper.Distribution(chatUnit, message.ChatInfoTree, false);
        
        if (result != 0)
        {
            Log.Warning($"Other2Chat_ChatMessageHandler: Distribution failed, result: {result}");
        }

        await FTask.CompletedTask;
    }
}