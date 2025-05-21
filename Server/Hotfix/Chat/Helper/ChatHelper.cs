using Fantasy.Platform.Net;

namespace Fantasy;

public static class ChatHelper
{
    /// <summary>
    /// 发送一个聊天消息给ChatScene（不能在ChatScene中调用）
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="chatUnitRouteId"></param>
    /// <param name="tree"></param>
    public static void SendChatMessage(Scene scene, long chatUnitRouteId, ChatInfoTree tree)
    {
        if (scene.SceneType == SceneType.Chat)
        {
            Log.Warning("ChatHelper.SendChatMessage: scene is not a chat scene.");
            return;
        }

        var other2ChatChatMessage = new Other2Chat_ChatMessage()
        {
            ChatInfoTree = tree
        };
        
        scene.NetworkMessagingComponent.SendInnerRoute(chatUnitRouteId, other2ChatChatMessage);
    }
}