using Fantasy.Async;
using Fantasy.Event;
using UnityEngine;

namespace GameLogic
{
    public class NetworkInitCompleted_Event : AsyncEventSystem<NetworkInitCompleted>
    {
        protected override async FTask Handler(NetworkInitCompleted self)
        {
            Debug.Log("网络初始化完成");
            GameModule.UI.ShowUIAsync<LobbyWindow>();
            //GameModule.UI.ShowUIAsync<UI_NetWork>();
        }
    }
}