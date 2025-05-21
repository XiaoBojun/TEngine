using System.Text;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;

namespace Fantasy
{
    public sealed class Chat2C_MessageHandler : Message<Chat2C_Message>
    {
        protected override async FTask Run(Session session, Chat2C_Message message)
        {
            ChatTreeParser.Parse(session.Scene,message.ChatInfoTree);
            Log.Info("收到聊天信息：");
            await FTask.CompletedTask;
        }
    }

    public static class ChatTreeParser
    {
        public static void Parse(Scene scene, ChatInfoTree tree)
        {
            var entryComponent = scene.GetComponent<EntryComponent>();
            var sb = new StringBuilder();
            foreach (var chatInfoNode in tree.Node)
            {
                // 这里只是演示一下处理事件的效果，实际使用时，需要根据实际情况处理事件
                // 明显我现在这样做的方式不是对的，应该是自己拼接一个聊天信息，然后调用这个接口来处理事件
                entryComponent.Entry.ChatNodeEventButton.onClick.RemoveAllListeners();
                entryComponent.Entry.ChatNodeEventButton.onClick.AddListener(() =>
                {
                    ChatNodeEventHelper.Handler(scene, chatInfoNode);
                });
                sb.Append(chatInfoNode.Content);
            }
            entryComponent.Entry.MessageText.text  = sb.ToString();
        }
    }
}