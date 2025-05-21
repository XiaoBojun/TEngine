using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Platform.Net;

namespace Fantasy;

public static class GateLoginHelper
{
    private static readonly List<Func<Scene, GateUnit, long, FTask<(uint, long, int)>>> Scenes =
        new List<Func<Scene, GateUnit, long, FTask<(uint, long, int)>>>()
        {
            // 聊天服务器上线
            { OnlineChat },
            // 游戏服务器上线
            { OnlineGame }
        };

    /// <summary>
    /// 上线到其他服务器
    /// </summary>
    /// <param name="session"></param>
    /// <param name="gateUnit"></param>
    /// <param name="gateRouteId"></param>
    public static async FTask<uint> Online(Session session, GateUnit gateUnit, long gateRouteId)
    {
        var scene = session.Scene;
        // 要实现自动Route转发协议，必须要给Session添加一个RouteComponent，然后通过它来转发消息。
        var routeComponent = session.GetOrAddComponent<RouteComponent>();
        // 这里是登录的总入口，在这里会陆续的登录其他服务器，如:聊天服务器、游戏服务器等
        foreach (var sceneHandler in Scenes)
        {
            var (errorCode, routeId, routeType) = await sceneHandler(scene, gateUnit, gateRouteId);
            if (errorCode != 0)
            {
                return errorCode;
            }
            // 保存上线过的RouteId，用于下线时通知其他服务器下线
            gateUnit.Routes[routeType] = routeId;
            // 添加到路由地址中，只有添加了这个路由映射地址，才会自动的从Gate转发到Chat
            routeComponent.AddAddress(routeType, routeId);
        }
        return 0;
    }

    /// <summary>
    /// 通知其他服务器下线
    /// </summary>
    
    /// <param name="gateUnit"></param>
    public static async FTask<uint> Offline(GateUnit gateUnit)
    {
        var networkMessagingComponent = gateUnit.Scene.NetworkMessagingComponent;
        
        foreach (var (routeType, routeId) in gateUnit.Routes)
        {
            switch (routeType)
            {
                case RouteType.ChatRoute:
                {
                    var response =
                        (Chat2G_OfflineResponse)await networkMessagingComponent.CallInnerRoute(routeId,
                            new G2Chat_OfflineRequest());
                    if (response.ErrorCode != 0)
                    {
                        return response.ErrorCode;
                    }

                    continue;
                }
            }
        }

        gateUnit.Routes.Clear();
        return 0;
    }

    /// <summary>
    /// 聊天服务器上线
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="gateUnit"></param>
    /// <param name="gateRouteId"></param>
    /// <returns></returns>
    private static async FTask<(uint errorCode, long routeId, int routeType)> OnlineChat(Scene scene, GateUnit gateUnit, long gateRouteId)
    {
        // 登录聊天服务器
        var chatConfig = SceneConfigData.Instance.GetSceneBySceneType(SceneType.Chat)[0];
        // 咱们框架中，如果要Scene和Scene之间通讯，必须要用到NetworkMessagingComponent，通过它来发送，没有其他办法。
        var response = (Chat2G_LoginResponse)await scene.NetworkMessagingComponent.CallInnerRoute(chatConfig.RouteId,
            new G2Chat_LoginRequest()
            {
                UserName = gateUnit.UserName,
                UnitId = gateUnit.Id,
                GateRouteId = gateRouteId
            });
        return (response.ErrorCode, response.ChatRouteId, RouteType.ChatRoute);
    }

    /// <summary>
    /// 游戏服务器上线
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="gateUnit"></param>
    /// <param name="gateRouteId"></param>
    /// <returns></returns>
    private static async FTask<(uint errorCode, long routeId, int routeType)> OnlineGame(Scene scene, GateUnit gateUnit, long gateRouteId)
    {
        // 登录聊天服务器
        var mapConfig = SceneConfigData.Instance.GetSceneBySceneType(SceneType.Map)[0];
        var response = (M2G_LoginResponse)await scene.NetworkMessagingComponent.CallInnerRoute(mapConfig.RouteId,
            new G2M_LoginRequest()
            {
                ChatUnitRouteId = gateUnit.Routes[RouteType.ChatRoute]
            });
        return (response.ErrorCode, response.MapRouteId, RouteType.GateRoute);
    }
}