namespace Fantasy;

public static class ChatChannelCenterHelper
{
    /// <summary>
    /// 申请一个频道
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="channelId"></param>
    /// <returns></returns>
    public static ChatChannelComponent Apply(Scene scene, long channelId)
    {
        return scene.GetComponent<ChatChannelCenterComponent>().Apply(channelId);
    }

    /// <summary>
    /// 尝试获取一个频道
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="channelId"></param>
    /// <param name="channel"></param>
    /// <returns></returns>
    public static bool TryGet(Scene scene, long channelId, out ChatChannelComponent channel)
    {
        return scene.GetComponent<ChatChannelCenterComponent>().TryGet(channelId, out channel);
    }

    /// <summary>
    /// 解散一个频道
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="channelId"></param>
    public static void Disband(Scene scene, long channelId)
    {
        scene.GetComponent<ChatChannelCenterComponent>().Disband(channelId);
    }
}