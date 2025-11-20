using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using UnityEngine;
using UnityEngine.UI;
using Log = TEngine.Log;

namespace GameLogic
{
	[Window(UILayer.UI, location : "UI_NetWork")]
	public class UI_NetWork : UIWindow
	{
		#region 脚本工具生成的代码

		private UI_NetWorkDataComponent m_bindComponent;
		private Text m_textMessage;
		private Button m_btnConnentServer;
		private Button m_btnSend;
		private Button m_btnSendRPC;
		private Button m_btnPushMessage;
		private Button m_btnReceive;
		private Button m_btnLoginAddress;
		private Button m_btnSendAddress;
		private Button m_btnSendAddressRPC;
		private Button m_btnReceiveAddress;

		protected override void ScriptGenerator()
		{
			m_bindComponent = gameObject.GetComponent<UI_NetWorkDataComponent>();
			m_textMessage = m_bindComponent.m_textMessage;
			m_btnConnentServer = m_bindComponent.m_btnConnentServer;
			m_btnSend = m_bindComponent.m_btnSend;
			m_btnSendRPC = m_bindComponent.m_btnSendRPC;
			m_btnPushMessage = m_bindComponent.m_btnPushMessage;
			m_btnReceive = m_bindComponent.m_btnReceive;
			m_btnLoginAddress = m_bindComponent.m_btnLoginAddress;
			m_btnSendAddress = m_bindComponent.m_btnSendAddress;
			m_btnSendAddressRPC = m_bindComponent.m_btnSendAddressRPC;
			m_btnReceiveAddress = m_bindComponent.m_btnReceiveAddress;
			m_btnConnentServer.onClick.AddListener(OnClickConnentServerBtn);
			m_btnSend.onClick.AddListener(OnClickSendBtn);
			m_btnSendRPC.onClick.AddListener(OnClickSendRPCBtn);
			m_btnPushMessage.onClick.AddListener(OnClickPushMessageBtn);
			m_btnReceive.onClick.AddListener(OnClickReceiveBtn);
			m_btnLoginAddress.onClick.AddListener(OnClickLoginAddressBtn);
			m_btnSendAddress.onClick.AddListener(OnClickSendAddressBtn);
			m_btnSendAddressRPC.onClick.AddListener(OnClickSendAddressRPCBtn);
			m_btnReceiveAddress.onClick.AddListener(OnClickReceiveAddressBtn);
		}



		#endregion


		#region 事件

		private void OnClickSendBtn()
		{
			Log.Debug($"OnSendMessageButtonClick");
			m_btnSend.interactable = false;
			FTModule.Session.C2G_TestMessage("Hello C2G_TestMessage");
			m_btnSend.interactable = true;
		}

		private void OnClickSendRPCBtn()
		{
			OnSendRPCMessageButtonClick().Coroutine();
		}
		private async FTask OnSendRPCMessageButtonClick()
		{
			m_btnSendRPC.interactable = false;
			// 发送一个RPC消息
			// C2G_TestRequest:服务器接收的协议
			// G2C_TestResponse:客户端接收到服务器发送的返回消息
			var response = await FTModule.Session.C2G_TestRequest("Hello C2G_TestRequest");
			m_textMessage.text = $"收到G2C_TestResponse Tag = {response.Tag}";
			m_btnSendRPC.interactable = true;
		}

		private void OnClickPushMessageBtn()
		{
			m_btnPushMessage.interactable = false;
			// 发送消息后，服务器会主动推送一个G2C_PushMessage消息给客户端。
			// 接收的Handler参考G2C_PushMessageHandler.cs。
			FTModule.Session.C2G_TestRequestPushMessage();
			m_btnPushMessage.interactable = true;
		}
		private void OnClickReceiveBtn()
		{
			
		}

		private void OnClickLoginAddressBtn()
		{
		}

		private void OnClickSendAddressBtn()
		{
		}

		private void OnClickSendAddressRPCBtn()
		{
		}

		private void OnClickReceiveAddressBtn()
		{
		}

		private void OnClickConnentServerBtn()
		{
			FTModule.Instance.Connect("127.0.0.1:20000", NetworkProtocolType.KCP, OnConnectComplete, OnConnectFail, OnConnectDisconnect);
		}

		#endregion
		
		
		public bool IsConnect;
		private void OnConnectComplete()
		{
			m_btnConnentServer.interactable = false;
			IsConnect = true;
			Log.Debug("已连接到服务器");
		}

		private void OnConnectFail()
		{
			m_btnConnentServer.interactable = true;
			IsConnect = false;
			Log.Error("无法连接到服务器");
		}
        
		private void OnConnectDisconnect()
		{
			m_btnConnentServer.interactable = true;
			IsConnect = false;
			Log.Error("服务器主动断开了连接");
		}
	}
	
	
}
