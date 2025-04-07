using UnityEngine;
using UnityEngine.UI;
using TEngine;

namespace GameLogic
{
    [Window(UILayer.UI)]
    class UI_AbilityWindow : UIWindow
    {
        #region 脚本工具生成的代码
        private GameObject _goAbilities;
        private Button _btnAccept;
        private Button _btnCancel;
        protected override void ScriptGenerator()
        {
            _goAbilities = FindChild("AbilityWindow/m_goAbilities").gameObject;
            _btnAccept = FindChildComponent<Button>("AbilityWindow/Buttons/Accept/m_btnAccept");
            _btnCancel = FindChildComponent<Button>("AbilityWindow/Buttons/Cancel/m_btnCancel");
            _btnAccept.onClick.AddListener(OnClickAcceptBtn);
            _btnCancel.onClick.AddListener(OnClickCancelBtn);
        }
        #endregion

        #region 事件
        private void OnClickAcceptBtn()
        {
        }
        private void OnClickCancelBtn()
        {
        }
        #endregion

        protected override void OnCreate()
        {
            base.OnCreate();
            var length=_goAbilities.transform.childCount;
            for (int i = 0; i < length; i++)
            {
                var ability = _goAbilities.transform.GetChild(i);

            }
        }
    }
}