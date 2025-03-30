using UnityEngine;
using UnityEngine.UI;
using TEngine;

namespace GameLogic
{
    [Window(UILayer.UI)]
    class UI_HUD : UIWindow
    {
       
        #region 脚本工具生成的代码
        private Text _textTime;
        private Button _btnUpgradeDamage;
        private Button _btnUpgradeSpeed;
        private Text _textMoney;
        private Text _textLevel;
        protected override void ScriptGenerator()
        {
            _textTime = FindChildComponent<Text>("CountDown/m_textTime");
            _btnUpgradeDamage = FindChildComponent<Button>("Stats/Skills/Damage/m_btnUpgradeDamage");
            _btnUpgradeSpeed = FindChildComponent<Button>("Stats/Skills/Speed/m_btnUpgradeSpeed");
            _textMoney = FindChildComponent<Text>("Stats/Info/m_textMoney");
            _textLevel = FindChildComponent<Text>("Stats/Info/m_textLevel");
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

        public Color highlightColor; //color when a skill is selected #B23838
        protected override void OnCreate()
        {
            base.OnCreate();
            AddUIEvent<string>(IUI_HUD_Event.Update_Level, Update_Level);
            AddUIEvent<int>(IUI_HUD_Event.Update_Money, Update_Money);
        }

        private void Update_Money(int money)
        {
            _textMoney.text =  "Money: " + money.ToString (); ;
        }

        private void Update_Level(string level)
        {
            _textLevel.text ="Enemy Level: " + level.ToString ();;
        }
    }
}