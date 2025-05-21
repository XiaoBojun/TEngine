using Fantasy.Entitas;
// ReSharper disable ArrangeObjectCreationWhenTypeEvident
// ReSharper disable UsageOfDefaultStructEquality

namespace Fantasy;

/// <summary>
/// 聊天频道实体
/// 1、根据频道内的玩家进行广播聊天信息。
/// 2、当前频道如果没有玩家的话，则自动销毁。
/// 3、存放当前频道的玩家信息。
/// </summary>
public sealed class ChatChannelComponent : Entity
{
    public readonly HashSet<long> Units = new HashSet<long>();
}