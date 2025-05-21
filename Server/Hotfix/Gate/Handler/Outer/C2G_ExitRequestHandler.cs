using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

namespace Fantasy;

public sealed class C2G_ExitRequestHandler : MessageRPC<C2G_ExitRequest, G2C_ExitResponse>
{
    protected override async FTask Run(Session session, C2G_ExitRequest request, G2C_ExitResponse response, Action reply)
    {
        session.RemoveComponent<GateUnitFlagComponent>();
        await FTask.CompletedTask;
    }
}