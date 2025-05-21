namespace Fantasy;

/// <summary>
/// 创建聊天树的总入口
/// </summary>
public static class ChatTreeFactory
{
    /// <summary>
    /// 创建世界聊天树
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    public static ChatInfoTree World(Scene scene)
    {
        return new ChatInfoTree()
        {
            Scene = scene,
            ChatChannelType = (int)ChatChannelType.World,
        };
    }

    /// <summary>
    /// 创建私聊聊天树
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    public static ChatInfoTree Private(Scene scene)
    {
        return new ChatInfoTree()
        {
            Scene = scene,
            ChatChannelType = (int)ChatChannelType.Private,
        };
    }

    /// <summary>
    /// 创建系统聊天树
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    public static ChatInfoTree System(Scene scene)
    {
        return new ChatInfoTree()
        {
            Scene = scene,
            ChatChannelType = (int)ChatChannelType.System,
        };
    }
    
    /// <summary>
    /// 创建公广播聊天树
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    public static ChatInfoTree Broadcast(Scene scene)
    {
        return new ChatInfoTree()
        {
            Scene = scene,
            ChatChannelType = (int)ChatChannelType.Broadcast,
        };
    }
    
    /// <summary>
    /// 创建公告聊天树
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    public static ChatInfoTree Notice(Scene scene)
    {
        return new ChatInfoTree()
        {
            Scene = scene,
            ChatChannelType = (int)ChatChannelType.Notice,
        };
    }
    
    /// <summary>
    /// 创建队伍聊天树
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    public static ChatInfoTree Team(Scene scene)
    {
        return new ChatInfoTree()
        {
            Scene = scene,
            ChatChannelType = (int)ChatChannelType.Team,
        };
    }
    
    /// <summary>
    /// 创建附近人聊天树
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    public static ChatInfoTree Near(Scene scene)
    {
        return new ChatInfoTree()
        {
            Scene = scene,
            ChatChannelType = (int)ChatChannelType.Near,
        };
    }
    
    /// <summary>
    /// 创建当前地图聊天树
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    public static ChatInfoTree CurrentMap(Scene scene)
    {
        return new ChatInfoTree()
        {
            Scene = scene,
            ChatChannelType = (int)ChatChannelType.CurrentMap,
        };
    }
}