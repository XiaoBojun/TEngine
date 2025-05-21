using Fantasy.Entitas;
using Fantasy.Entitas.Interface;

#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8601 // Possible null reference assignment.

namespace Fantasy;

public sealed class ChatChannelCenterComponentDestroySystem :  DestroySystem<ChatChannelCenterComponent>
{
    protected override void Destroy(ChatChannelCenterComponent self)
    {
        foreach (var chatChannelComponent in self.Channels.Values.ToArray())
        {
            chatChannelComponent.Dispose();
        }
        self.Channels.Clear();
    }
}

public static class ChatChannelCenterComponentSystem
{
    public static ChatChannelComponent Apply(this ChatChannelCenterComponent self, long channelId)
    {
        if (self.Channels.TryGetValue(channelId, out var channel))
        {
            return channel;
        }

        channel = Entity.Create<ChatChannelComponent>(self.Scene, channelId, true, true);
        self.Channels.Add(channelId, channel);
        return channel;
    }

    public static bool TryGet(this ChatChannelCenterComponent self, long channelId, out ChatChannelComponent channel)
    {
        return self.Channels.TryGetValue(channelId, out channel);
    }

    public static void Disband(this ChatChannelCenterComponent self, long channelId)
    {
        if (self.Channels.Remove(channelId, out var channel))
        {
            return;
        }

        channel.Dispose();
    }
}