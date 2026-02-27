using TMPro;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TEngine;
using AudioType = TEngine.AudioType;

namespace GameLogic
{
	[Window(UILayer.UI, location : "LobbyWindow")]
	public partial class LobbyWindow
	{
		#region 事件

		private async partial UniTaskVoid OnClickSingleBtn()
		{
			await UniTask.Yield();
		}

		private async partial UniTaskVoid OnClickCreateRoomBtn()
		{
			await UniTask.Yield();
		}

		private async partial UniTaskVoid OnClickbtnMatchBtn()
		{
			GameModule.Audio.Play(AudioType.UISound, Constants.NormalClick);
			
			await UniTask.Yield();
		}

		#endregion
	}
}
