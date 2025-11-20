using System;
using System.Reflection;
using Fantasy;
using Fantasy.Async;
using Fantasy.Helper;
using Fantasy.Network;
using Fantasy.Network.Interface;
using UnityEngine;

namespace GameLogic
{
    public sealed partial class FTModule : Singleton<FTModule>, IUpdate
    {
        /// <summary>
        /// Fantasy Scene
        /// </summary>
        private Scene _scene;

        public static Scene Scene;

        /// <summary>
        /// 网络会话Socket
        /// </summary>
        private Session _session;

        public static Session Session;

        /// <summary>
        /// 心跳检测
        /// </summary>
        public int mHeartBeatInterval = 5000;


        /// <summary>
        /// 初始化网络层
        /// </summary>
        public async FTask Initialize(params Assembly[] assemblies)
        {
            assemblies[0].EnsureLoaded();
            assemblies[1].EnsureLoaded();
            // 初始化程序集管理系统
            await Fantasy.Platform.Unity.Entry.Initialize();
            // 精度处理（只针对Windows下有作用、其他系统没有这个问题、一般也不会用Windows来做服务器的）
            WinPeriod.Initialize();
            // 框架需要一个Scene来驱动、所以要创建一个Scene、后面所有的框架都会在这个Scene下
            // 也就是把这个Scene给卸载掉、框架的东西都会清除
            _scene = await Scene.Create();
            Scene = _scene;
            await Scene.EventComponent.PublishAsync(new NetworkInitCompleted());
        }


        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="romoteAddress"></param>
        /// <param name="networkProtocolType"></param>
        /// <returns></returns>
        public void Connect(string romoteAddress, NetworkProtocolType networkProtocolType,
            Action onConnectComplete = null,
            Action onConnectFail = null,
            Action onConnectDisconnect = null,
            int connectOutTime = 5000)
        {
            _session = _scene.Connect(romoteAddress, networkProtocolType,
                () =>
                {
                    Session = _session;
                    ConnectSuccess();
                    onConnectComplete?.Invoke();
                },
                () =>
                {
                    ConnectFailure();
                    onConnectFail?.Invoke();
                },
                () =>
                {
                    ConnectDisConnect();
                    onConnectDisconnect?.Invoke();
                },
                false,
                connectOutTime);
            Session = _session;
        }

        public void DisConnect()
        {
            _session?.Dispose();
        }


        /// <summary>
        /// 链接服务器成功
        /// </summary>
        private void ConnectSuccess()
        {
            _session.AddComponent<SessionHeartbeatComponent>().Start(mHeartBeatInterval);
        }

        /// <summary>
        /// 链接服务器失败
        /// </summary>
        private void ConnectFailure()
        {
        }

        /// <summary>
        /// 服务器链接终端 在处于链接状态中，Socket断掉了会触发这个接口
        /// </summary>
        private void ConnectDisConnect()
        {
        }


        /// <summary>
        /// 释放网络层
        /// </summary>
        protected override void OnRelease()
        {
            base.OnRelease();
            _session?.Dispose();

            _scene?.Dispose();
        }

        public void OnUpdate()
        {
            //ThreadScheduler.Update();
        }
        // private void OnApplicationQuit()
        // {
        //     AssemblySystem.Dispose();
        //     //SingletonSystem.Instance.Dispose();
        //     Scene?.Dispose();
        // }
    }
}