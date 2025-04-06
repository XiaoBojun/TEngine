using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace GameLogic
{
    [Window(UILayer.UI)]
    class Menu : UIWindow
    {
        #region 脚本工具生成的代码
        private Button _btnNewGame;
        private Button _btnContinue;
        protected override void ScriptGenerator()
        {
            _btnNewGame = FindChildComponent<Button>("m_btnNewGame");
            _btnContinue = FindChildComponent<Button>("m_btnContinue");
            _btnNewGame.onClick.AddListener(OnClickNewGameBtn);
            _btnContinue.onClick.AddListener(OnClickContinueBtn);
        }
        #endregion

        public static bool newGame = true; 
        #region 事件
        private void OnClickNewGameBtn()
        {
            newGame = true;
            EnterGameScene().Forget();
        }

        private async UniTaskVoid EnterGameScene()
        {
            var scene = await  GameModule.Scene.LoadSceneAsync("Game");
            if (scene.isLoaded)
            {
                AnldleGame.Instance.OnEnterGame();
                
                var camera = Camera.main;
                 UniversalAdditionalCameraData baseCameraData = camera.GetUniversalAdditionalCameraData();
                baseCameraData.cameraStack.Add(GameModule.UI.UICamera);
                
                Close();
            }
        }
        
        private void OnClickContinueBtn()
        {
            newGame = false;
            GameModule.Scene.LoadScene("Game");
            Close();
        }
        #endregion

    }
}