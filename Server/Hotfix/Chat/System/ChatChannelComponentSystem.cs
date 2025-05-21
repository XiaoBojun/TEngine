using Fantasy.Entitas;

namespace Fantasy;

public class MemoryEntity : Entity
{
    
}
    

public static class ChatChannelComponentSystem
{
    public static void Send(this ChatChannelComponent self, ChatInfoTree tree)
    {
        var chatUnitManageComponent = self.Scene.GetComponent<ChatUnitManageComponent>();
        var networkMessagingComponent = self.Scene.NetworkMessagingComponent;
        var chatMessage = new Chat2C_Message()
        {
            ChatInfoTree = tree
        };
        
        foreach (var unitId in self.Units)
        {
            if (!chatUnitManageComponent.Units.TryGetValue(unitId, out var chatUnit))
            {
                continue;
            }

            networkMessagingComponent.SendInnerRoute(chatUnit.GateRouteId, chatMessage);
        }
    }
    
    public static bool JoinChannel(this ChatChannelComponent self, long chatUnitId)
    {
        var chatUnitManageComponent = self.Scene.GetComponent<ChatUnitManageComponent>();

        if (!chatUnitManageComponent.TryGet(chatUnitId, out var chatUnit))
        {
            return false;
        }

        // 将当前频道中加入该用户。
        self.Units.Add(chatUnitId);
        // 给用户添加频道。
        if (!chatUnit.Channels.ContainsKey(self.Id))
        {
            chatUnit.Channels.Add(self.Id, self);
        }
        // 可以在这里给客户端发送一个加入频道成功的消息。
        return true;
    }

    public static bool IsJoinedChannel(this ChatChannelComponent self, long chatUnitId)
    {
        return self.Units.Contains(chatUnitId);
    }

    public static void ExitChannel(this ChatChannelComponent self, long chatUnitId, bool isRemoveUnitChannel = true)
    {
        if (!self.Units.Contains(chatUnitId))
        {
            return;
        }

        var chatUnitManageComponent = self.Scene.GetComponent<ChatUnitManageComponent>();

        if (!chatUnitManageComponent.TryGet(chatUnitId, out var chatUnit))
        {
            return;
        }

        if (isRemoveUnitChannel)
        {
            // 给用户移除频道。
            chatUnit.Channels.Remove(self.Id);
        }
        
        // 在当前频道中移除该用户。
        self.Units.Remove(chatUnitId);
        // 如果当前频道中没有用户了，则销毁该频道。
        if (self.Units.Count == 0)
        {
            self.Dispose();
        }
    }
}