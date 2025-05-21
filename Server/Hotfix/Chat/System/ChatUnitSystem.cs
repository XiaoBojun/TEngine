using Fantasy.Entitas.Interface;
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

namespace Fantasy;

public sealed class ChatUnitDestroySystem : DestroySystem<ChatUnit>
{
    protected override void Destroy(ChatUnit self)
    {
        self.UserName = null;
        self.GateRouteId = 0;
        // 退出当前ChatUnit拥有的所有频道
        foreach (var (_,chatChannelComponent) in self.Channels)
        {
            chatChannelComponent.ExitChannel(self.Id, false);
        }
        // 理论情况下，这个self.Channels不会存在因为数据的，因为上面已经给清空掉了。
        // 但是self.Channels.Clear();还是加上吧，防止以后忘记了。
        self.Channels.Clear();
    }
}