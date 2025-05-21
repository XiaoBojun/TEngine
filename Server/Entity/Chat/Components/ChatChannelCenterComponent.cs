using Fantasy.Entitas;

namespace Fantasy;

/// <summary>
/// 聊天中控中心
/// 1、申请、创建、解散聊天频道。
/// 2、管理聊天频道成员。
/// 3、根据频道ID找到对应的频道。
/// </summary>
public class ChatChannelCenterComponent : Entity
{
    public readonly Dictionary<long, ChatChannelComponent> Channels = new();
}