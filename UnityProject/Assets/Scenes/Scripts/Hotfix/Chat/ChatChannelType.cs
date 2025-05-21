using System;

namespace Fantasy
{
    /// <summary>
    /// 聊天频道类型
    /// </summary>
    [Flags]
    public enum ChatChannelType
    {
        None                   = 0,
        World                  = 1 << 1,    // 世界频道
        Private                = 1 << 2,    // 私聊频道
        System                 = 1 << 3,    // 系统频道
        Broadcast              = 1 << 4,    // 广播频道
        Notice                 = 1 << 5,    // 公告频道
        Team                   = 1 << 6,    // 队伍频道
        Near                   = 1 << 7,    // 附近频道
        CurrentMap             = 1 << 8,    // 当前地图频道
    
        // 所有频道
        All                    = World | Private | System | Broadcast | Notice | Team | Near,
        // 其他聊天栏显示的频道
        Display                = World | Private | System | Broadcast | Notice | Team | Near | CurrentMap
    }

    /// <summary>
    /// 聊天节点类型
    /// </summary>
    public enum ChatNodeType
    {
        None                   = 0,
        Position               = 1,    // 位置节点
        OpenUI                 = 2,    // 打开UI节点
        Link                   = 3,    // 链接节点
        Item                   = 4,    // 物品节点
        Text                   = 5,    // 文本节点
        Image                  = 6,    // 图片节点
    }
    
    /// <summary>
    /// 聊天节点事件类型
    /// </summary>
    public enum ChatNodeEvent
    {
        None = 0,
        OpenUI = 1,              // 打开UI节点
        ClickLink = 2,           // 点击链接节点  
        UseItem = 3,             // 使用物品节点
        Position = 4,            // 位置节点
    }
}