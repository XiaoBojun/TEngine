using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TEngine;

namespace GameLogic
{
    [Window(UILayer.UI)]
    class UI_Menu : UIWindow
    {
        #region 脚本工具生成的代码

        private Button _btnRPG;

        protected override void ScriptGenerator()
        {
            _btnRPG = FindChildComponent<Button>("m_btnRPG");
            _btnRPG.onClick.AddListener(UniTask.UnityAction(OnClickRPGBtn));
        }

        #endregion

        #region 事件

        private async UniTaskVoid OnClickRPGBtn()
        { 
            await GameModule.Scene.LoadSceneAsync("RpgExample");
            await GameModule.Resource.LoadGameObjectAsync("AppLoad");
            Hide();
            GameModule.UI.ShowUIAsync<UI_Battle>();
            // AppLoad.Instance.ConfigsCollector=await GameModule.Resource.LoadAssetAsync<ReferenceCollector>("Configs");
            // AppLoad.Instance.PrefabsCollector=await GameModule.Resource.LoadAssetAsync<ReferenceCollector>("Prefabs");
           // AppLoad.Instance.Init();
            await UniTask.Yield();
        }

        #endregion
    }
}