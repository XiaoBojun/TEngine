using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;

namespace Fantasy;

public sealed class C2G_LoginRequestHandler : MessageRPC<C2G_LoginRequest, G2C_LoginResponse>
{
    protected override async FTask Run(Session session, C2G_LoginRequest request, G2C_LoginResponse response, Action reply)
    {
        if (string.IsNullOrEmpty(request.UserName))
        {
            // 这里返回的1代表账号信息是空的。
            response.ErrorCode = 1;
            return;
        }

        var scene = session.Scene;
        // 添加一个GateUnitFlagComponent组件，用来Session断开、或者传递数据时使用。
        var gateUnitFlagComponent = session.GetOrAddComponent<GateUnitFlagComponent>();
        // 上线到Gate
        var gateUnit = GateUnitHelper.Online(scene, request.UserName, session);
        // 设置GateUnitFlagComponent的GateUnitId
        gateUnitFlagComponent.GateUnitId = gateUnit.Id;
        // 登录到其他服务器
        await GateLoginHelper.Online(session, gateUnit, session.RuntimeId);//RunTimeId
        Log.Debug($"gateUnit : {gateUnit.UserName}");
        await FTask.CompletedTask;
    }
}