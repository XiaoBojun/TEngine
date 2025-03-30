using UnityEngine;
using UnityEngine.UI;
using TEngine;
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
            GameModule.Scene.LoadScene("Game", UnityEngine.SceneManagement.LoadSceneMode.Single,false,100,OnToSceneSuccess);
            Close();
        }

        private void OnToSceneSuccess(Scene scene)
        {
            AnldleGame.Instance.OnEnterGame();
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