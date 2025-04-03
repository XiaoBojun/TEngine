using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TEngine;

namespace GameLogic
{
    [Window(UILayer.UI)]
    class UI_ActivatedSkill : UIWindow
    {
        #region 脚本工具生成的代码
        private Image _imgNewSkillAlarm;
        private Image _imgCooldown;
        private Text _textPopup;
        protected override void ScriptGenerator()
        {
            _imgNewSkillAlarm = FindChildComponent<Image>("m_imgNewSkillAlarm");
            _imgCooldown = FindChildComponent<Image>("Icon/m_imgCooldown");
            _textPopup = FindChildComponent<Text>("m_textPopup");
        }
        #endregion

        #region 事件
        
        #endregion

        
        [HideInInspector]
        public ActiveSkill activeSkill; //reference to the current using active skill

        [HideInInspector]
        public ActiveSkill selectedActiveSkill; //used when you select a skill in active skill selecting window
        private float timer; //when this number is bigger than attackInterval, the character attacks
        private bool isCooldown = false;
        protected override void OnCreate()
        {
            base.OnCreate();
            foreach (ActiveSkill activeSkill in AnldleGame.Instance.activeSkills) //load the activeSkill which has the saved ID
            {
            	if (activeSkill.ID == AnldleGame.Instance.playerData.activatedSkillID)
            	{
            		this.activeSkill = selectedActiveSkill = activeSkill;
            
            		break;
            	}
            }
            
            AddUIEvent(IUI_ActivatedSkill_Event.Action_WaitForCooldown,Action_WaitForCooldown);
            UpdateSkillText(); //call the function to update the text with corresponding damage and speed level to the actived skill
            
            //Money = AnldleGame.Instance.playerData.money; //load the amount of money
            
            TimeSpan timeSinceLastPlay = DateTime.Now - AnldleGame.Instance.playerData.quitTime; //calculate the time difference between last play and now
            
            //if (timeSinceLastPlay > TimeSpan.Zero) //if the difference is not negative
            	//Money += (int)(timeSinceLastPlay.TotalHours * Mathf.Pow(AnldleGame.Instance.playerData.level, 2f)); //gain money according to the time difference
        }
        public void UpgradeDamage() //function get called when the player click the damage upgrade button
        {
            // if (Money >= activeSkill.DamageUpgradeCost) //if the player has enough money
            // {
            // 	Money -= activeSkill.DamageUpgradeCost;
            //
            // 	activeSkill.DamageLevel += 1;
            //
            // 	//damageText.UpdateSkillText(activeSkill.DamageLevel, activeSkill.DamageUpgradeCost); //update the text to show the new damage level and upgrade cost
            // }
            // else
            // {
            // 	Debug.Log("need more money");
            // }
        }

        public void UpgradeSpeed() //function to upgrade speed, similar to UpgradeDamage()
        {
            // if (Money >= activeSkill.SpeedUpgradeCost)
            // {
            // 	Money -= activeSkill.SpeedUpgradeCost;
            //
            // 	activeSkill.SpeedLevel += 1;
            //
            // 	//speedText.UpdateSkillText(activeSkill.SpeedLevel, activeSkill.SpeedUpgradeCost);
            // }
            // else
            // {
            // 	Debug.Log("need more money");
            // }
        }

        public void UpdateSkillText() //function to update the text of Lv. and costs for both damage and speed
        {
            //damageText.UpdateSkillText(activeSkill.DamageLevel, activeSkill.DamageUpgradeCost);

            //speedText.UpdateSkillText(activeSkill.SpeedLevel, activeSkill.SpeedUpgradeCost);
        }

        public void Action_WaitForCooldown()
        {
            WaitForCooldown().Forget();
        }    
        
        private async UniTaskVoid WaitForCooldown()
        {
            IsCoolDown(false);
            _imgCooldown.fillAmount = 1f;
            timer = 0f;
            isCooldown = true;

            while (isCooldown)
            {
                timer += Time.deltaTime;

                // 如果达到攻击间隔，则重置
                if (timer >= AnldleGame_Data.Instance.attackInterval)
                {
                    isCooldown = false;
                    _imgCooldown.fillAmount = 0f;
                    IsCoolDown(true);
                    break;
                }

                // 更新填充图像的填充量
                _imgCooldown.fillAmount = 1f - timer / AnldleGame_Data.Instance.attackInterval;

                // 等待下一帧
                await UniTask.Yield();
            }
        }
        /// <summary>
        /// 当前冷却
        /// </summary>
        /// <param name="_isCoolDown"></param>
        private void IsCoolDown(bool _isCoolDown)
        {
            AnldleGame_Data.Instance.isCoolDown = _isCoolDown;
        }
    }
}