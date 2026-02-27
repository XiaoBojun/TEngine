using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;

namespace Hotfix.Login;

public class C2G_LoginHandler: MessageRPC<C2G_Login, G2C_Login>
{
    protected override async FTask Run(Session session, C2G_Login request, G2C_Login response, Action reply)
    {
        var loginComponent =session.GetComponent<GateComponent>();
        if (loginComponent.IsAccountOnline(request.Acct))
        {
            response.ErrorCode = (uint)ErrorCode.AcctIsOnline;
            return;
        }
        PlayerData playerData = loginComponent.GetPlayerData(request.Acct, request.Pwd);
        if(playerData == null) {
            response.ErrorCode = (uint)ErrorCode.WrongPass;
            return;
        }
        else
        {
            response.playerData = playerData;
            // response.scoreRankLst = loginComponent;
            // response.arenaRankLst = loginComponent;
            
            //loginComponent.AcctOnLine(request.Acct, pack.token, playerData);
        }            
        
        await FTask.CompletedTask;
    }
}