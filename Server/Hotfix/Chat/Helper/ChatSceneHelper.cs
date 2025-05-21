using System.Threading.Channels;
using Fantasy.Helper;
using Fantasy.Platform.Net;
// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

namespace Fantasy;

public static class ChatSceneHelper
{
    private const int ChatCD = 1000;
    private const int MaxTextLength = 10;
    private const int MaxShowItemCount = 2;

    /// <summary>
    /// 聊天消息分发入口
    /// </summary>
    /// <param name="chatUnit"></param>
    /// <param name="tree"></param>
    /// <param name="isCheckSendTime"></param>
    /// <returns></returns>
    public static uint Distribution(ChatUnit chatUnit, ChatInfoTree tree, bool isCheckSendTime = true)
    {
        var result = Condition(chatUnit, tree, isCheckSendTime);

        if (result != 0)
        {
            return result;
        }
        
        switch ((ChatChannelType)tree.ChatChannelType)
        {
            case ChatChannelType.Broadcast:
            {
                Broadcast(chatUnit.Scene, tree);
                return 0;
            }
            case ChatChannelType.Team:
            {
                return Channel(chatUnit, tree);
            }
            case ChatChannelType.Private:
            {
                return Private(chatUnit, tree);
            }
            default:
            {
                // 这个1代表当前频道不存在。
                return 1;
            }
        }
    }

    /// <summary>
    /// 聊天消息条件判断
    /// </summary>
    /// <param name="chatUnit"></param>
    /// <param name="tree"></param>
    /// <param name="isCheckSendTime"></param>
    /// <returns></returns>
    private static uint Condition(ChatUnit chatUnit, ChatInfoTree tree, bool isCheckSendTime = true)
    {
        // 每个频道可能聊天的间隔都不一样。
        // 这里的条件判断，是根据频道的类型，来判断是否到达了聊天的间隔。
        var now = TimeHelper.Now;
        
        if (isCheckSendTime)
        {
            // 这里的间隔时间，是根据频道的类型，来获取的。
            chatUnit.SendTime.TryGetValue(tree.ChatChannelType, out var sendTime);
            // 判定聊天间隔是否到达
            // 其实的话，这个ChatCD应该是根据频道的类型，来获取的。
            // 一般的话都是做一个配置表，通过配置表来获取不同频道的时间间隔。
            if (now - sendTime < ChatCD)
            {
                // 这个1代表当前频道聊天的间隔过短
                return 1;
            }
        }

        // 判定聊天内容是否超长

        var itemCount = 0;
        var chatTextSize = 0;

        foreach (var chatInfoNode in tree.Node)
        {
            switch ((ChatNodeType)chatInfoNode.ChatNodeType)
            {
                case ChatNodeType.Text:
                {
                    chatTextSize += chatInfoNode.Content.Length;
                    break;
                }
                case ChatNodeType.Image:
                {
                    // 规定图片占聊天消息的5个字符长度
                    chatTextSize += 5;
                    break;
                }
                case ChatNodeType.OpenUI:
                {
                    // 规定OpenUI占聊天消息的10个字符长度
                    chatTextSize += 10;
                    break;
                }
                case ChatNodeType.Item:
                {
                    itemCount++;
                    break;
                }
            }
        }

        if (chatTextSize > MaxTextLength)
        {
            // 这个2代表当前频道聊天内容超长
            return 2;
        }

        if (itemCount > MaxShowItemCount)
        {
            // 这个3代表当前频道聊天里道具数量过多
            return 3;
        }

        if (isCheckSendTime)
        {
            // 更新当前频道的发送时间
            chatUnit.SendTime[tree.ChatChannelType] = now;
        }
        
        return 0;
    }

    #region 不同聊天频道的实现
    
    /// <summary>
    /// 广播消息
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="tree"></param>
    private static void Broadcast(Scene scene, ChatInfoTree tree)
    {
        var networkMessagingComponent = scene.NetworkMessagingComponent;
        var chatMessage = new Chat2G_ChatMessage()
        {
            ChatInfoTree = tree
        };
        
        if (tree.Target.Count > 0)
        {
            var chatUnitManageComponent = scene.GetComponent<ChatUnitManageComponent>();
            // 给一部分人广播消息
            foreach (var chatUnitId in tree.Target)
            {
                if (!chatUnitManageComponent.TryGet(chatUnitId, out var chatUnit))
                {
                    continue;
                }

                networkMessagingComponent.SendInnerRoute(chatUnit.GateRouteId, chatMessage);
            }
            return;
        }
        
        // 发送给所有Gate服务器，让Gate服务器转发给其他客户端
        var gateConfigs = SceneConfigData.Instance.GetSceneBySceneType(SceneType.Gate);
        foreach (var gateSceneConfig in gateConfigs)
        {
            // 这里是要发送一个消息给Gate服务器，Gate服务器再转发给其他客户端
            networkMessagingComponent.SendInnerRoute(gateSceneConfig.RouteId, chatMessage);
        }
    }

    /// <summary>
    /// 发送频道消息
    /// </summary>
    /// <param name="chatUnit"></param>
    /// <param name="tree"></param>
    private static uint Channel(ChatUnit chatUnit, ChatInfoTree tree)
    {
        // 那组队，公会、地图、等这个的聊天，如何使用频道呢?
        // 这里的频道，是指一个频道，比如一个公会的频道，一个队伍的频道，一个地图的频道。
        // 1、一般组队工会、地图等，都是有一个创建的一个逻辑，咱们个在这个创建的逻辑中，根据队伍、公会、地图的ID
        // 把这些ID当做频道ID,然后发送到Chat服务器，先申请一个频道，把这个频道ID返回给创建公会队伍的逻辑。
        // 这时候，队伍公会、发送聊天消息的时候，就会根据这个ID来进行发送。
        // 2、地图同样道理，创建地图的时候，也会有一个创建的逻辑，这个逻辑会返回一个地图的ID，这个ID就是地图的频道ID。
        // 3、这这些ID根据频道类型，发送给客户端，客户端发送的时候，根据频道不同，拿不同的ID来发送。
        
        // 课外:
        // 客户端创建一个频道、拿到这个频道号，告诉其他人，其他人通过这个频道ID加入到这个频道。
        // 客户端创建一个频道，邀请其他人加入到这个频道，其他人可能客户端会接收一个协议，就是邀请你加入到这个频道，如果同意，
        // 你就加入到这个频道（你点同意后，会发送一个消息给聊天服务器，聊天服务器会把你加入到这个频道）。
       
        if (!chatUnit.Channels.TryGetValue(tree.ChatChannelId, out var channel))
        {
            // 这个1代表当前频道不存在。
            return 1;
        }

        channel.Send(tree);
        return 0;
    }

    /// <summary>
    /// 发送私聊消息
    /// </summary>
    /// <param name="chatUnit"></param>
    /// <param name="tree"></param>
    /// <returns></returns>
    private static uint Private(ChatUnit chatUnit, ChatInfoTree tree)
    {
        // 私聊，就是两个玩家之间，直接聊天。
        // 1、首先，客户端需要知道对方的ID，这个ID是通过什么方式获取的呢？
        // 2、客户端需要发送一个私聊消息给聊天服务器，聊天服务器需要把这个消息转发给对方。
        // 3、对方收到消息后，需要显示出来。
        // 4、聊天服务器需要记录这个私聊消息，并把这个消息转发给两个玩家。
        // 5、两个玩家收到消息后，需要显示出来。

        if (tree.Target == null || tree.Target.Count <= 0)
        {
            // 这个1代表对方ID不的合法的。
            return 1;
        }

        var targetChatUnitId = tree.Target[0];
        var scene = chatUnit.Scene;
        if (!scene.GetComponent<ChatUnitManageComponent>().TryGet(targetChatUnitId, out var targetChatUnit))
        {
            // 这个2代表对方不在线。
            return 2;
        }

        var networkMessagingComponent = scene.NetworkMessagingComponent;
        var chatMessage = new Chat2C_Message()
        {
            ChatInfoTree = tree
        };
        
        // 先给自己发送一个聊天消息。
        networkMessagingComponent.SendInnerRoute(chatUnit.GateRouteId, chatMessage);
        // 然后再给对方发送一个聊天消息。
        networkMessagingComponent.SendInnerRoute(targetChatUnit.GateRouteId, chatMessage);
        return 0;
    }
    
    #endregion
}