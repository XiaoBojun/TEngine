using UnityEngine;
using UnityEngine.UI;
using TEngine;

namespace GameLogic
{
    [Window(UILayer.UI)]
    class UICanvas : UIWindow
    {
        public Color highlightColor; //color when a skill is selected #B23838
        #region 脚本工具生成的代码
        private Text _textTime;
        private Button _btnUpgradeDamage;
        private Button _btnUpgradeSpeed;
        private Text _textMoney;
        private Text _textLevel;
        protected override void ScriptGenerator()
        {
            _textTime = FindChildComponent<Text>("UI_HUD/CountDown/m_textTime");
            _btnUpgradeDamage = FindChildComponent<Button>("UI_HUD/Stats/Skills/Damage/m_btnUpgradeDamage");
            _btnUpgradeSpeed = FindChildComponent<Button>("UI_HUD/Stats/Skills/Speed/m_btnUpgradeSpeed");
            _textMoney = FindChildComponent<Text>("UI_HUD/Stats/Info/m_textMoney");
            _textLevel = FindChildComponent<Text>("UI_HUD/Stats/Info/m_textLevel");
            _btnUpgradeDamage.onClick.AddListener(OnClickUpgradeDamageBtn);
            _btnUpgradeSpeed.onClick.AddListener(OnClickUpgradeSpeedBtn);
        }
        #endregion

        #region 事件
        private void OnClickUpgradeDamageBtn()
        {
        }
        private void OnClickUpgradeSpeedBtn()
        {
        }
        #endregion

    }
}