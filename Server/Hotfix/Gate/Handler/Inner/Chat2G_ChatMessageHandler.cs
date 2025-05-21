using Fantasy.Async;
using Fantasy.Network.Interface;

namespace Fantasy;

public sealed class Chat2G_ChatMessageHandler : Route<Scene,Chat2G_ChatMessage>
{
    protected override async FTask Run(Scene scene, Chat2G_ChatMessage message)
    {
        var chatMessage = new Chat2C_Message()
        {
            ChatInfoTree = message.ChatInfoTree
        };
        foreach (var session in scene.GetComponent<GateUnitManageComponent>().ForEachUnitSession())
        {
            session.Send(chatMessage);
        }
        await FTask.CompletedTask;
    }
}