using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TEngine;

namespace GameLogic
{
    [Window(UILayer.UI)]
    class UI_Battle : UIWindow
    {
        #region 脚本工具生成的代码

        private Button _btnReturn;
        private Button _btnSkillA;
        private Button _btnSkillB;
        private Button _btnSkillC;
        private Button _btnSkillD;
        private Button _btnSkillE;
        private Button _btnSkillF;
        private Image _imgTargetSelect;

        protected override void ScriptGenerator()
        {
            _btnReturn = FindChildComponent<Button>("m_btnReturn");
            _btnSkillA = FindChildComponent<Button>("SkillSlots/m_btnSkillA");
            _btnSkillB = FindChildComponent<Button>("SkillSlots/m_btnSkillB");
            _btnSkillC = FindChildComponent<Button>("SkillSlots/m_btnSkillC");
            _btnSkillD = FindChildComponent<Button>("SkillSlots/m_btnSkillD");
            _btnSkillE = FindChildComponent<Button>("SkillSlots/m_btnSkillE");
            _btnSkillF = FindChildComponent<Button>("SkillSlots/m_btnSkillF");
            _imgTargetSelect = FindChildComponent<Image>("m_imgTargetSelect");
            _btnReturn.onClick.AddListener(UniTask.UnityAction(OnClickReturnBtn));
            _btnSkillA.onClick.AddListener(UniTask.UnityAction(OnClickSkillABtn));
            _btnSkillB.onClick.AddListener(UniTask.UnityAction(OnClickSkillBBtn));
            _btnSkillC.onClick.AddListener(UniTask.UnityAction(OnClickSkillCBtn));
            _btnSkillD.onClick.AddListener(UniTask.UnityAction(OnClickSkillDBtn));
            _btnSkillE.onClick.AddListener(UniTask.UnityAction(OnClickSkillEBtn));
            _btnSkillF.onClick.AddListener(UniTask.UnityAction(OnClickSkillFBtn));
        }

        #endregion

        #region 事件

        private async UniTaskVoid OnClickReturnBtn()
        {
            Hide();
            GameModule.UI.ShowUIAsync<UI_Menu>();
            await GameModule.Scene.LoadSceneAsync("Hall");
        }

        private async UniTaskVoid OnClickSkillABtn()
        {
            await UniTask.Yield();
        }

        private async UniTaskVoid OnClickSkillBBtn()
        {
            await UniTask.Yield();
        }

        private async UniTaskVoid OnClickSkillCBtn()
        {
            await UniTask.Yield();
        }

        private async UniTaskVoid OnClickSkillDBtn()
        {
            await UniTask.Yield();
        }

        private async UniTaskVoid OnClickSkillEBtn()
        {
            await UniTask.Yield();
        }

        private async UniTaskVoid OnClickSkillFBtn()
        {
            await UniTask.Yield();
        }

        #endregion

        protected override void OnCreate()
        {
            base.OnCreate();
            AddUIEvent<Color>(IUI_Battle_Event.SetCursorImageColor,SetCursorImageColor);
            AddUIEvent<bool>(IUI_Battle_Event.ShowCursorImage,ShowCursorImage);
        }

        private void ShowCursorImage(bool isShow)
        {
            _imgTargetSelect.gameObject.SetActive(isShow);
        }

        private void SetCursorImageColor(Color color)
        {
            _imgTargetSelect.color = color;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            _imgTargetSelect.transform.position = Input.mousePosition;
        }
    }
}