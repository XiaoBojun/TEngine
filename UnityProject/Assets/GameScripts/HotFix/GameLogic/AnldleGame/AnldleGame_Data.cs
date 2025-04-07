using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class AnldleGame_Data:Singleton<AnldleGame_Data>
    {
        /// <summary>
        /// 攻击间隔
        /// </summary>
        public float attackInterval;
        /// <summary>
        /// 冷却是否完成
        /// </summary>
        public bool isCoolDown;

        public int activeSkill_numberOfTargets;

        public int AttackDamage;
        public int activatedSkillID; //the ID of the active skill
        private int level; //the monster level
        public int Level
        {
            get { return level; }
            set
            {
                level = value;
                GameEvent.Send(IUI_HUD_Event.Update_Level, level);
            }
        }

        private int money; //money you have

        public int Money
        {
            get { return money; }
            set
            {
                money = value;
                GameEvent.Get<IUI_HUD>().Update_Money(money);
            }
        }

        [HideInInspector]
        public PlayerData playerData; //the selected active skill in active skill selecting window
        public void LevelAdd(int value=1)
        {
            Level += value;
        }
        public void MoneyAdd(int value=1)
        {
            Money += value;
        }
        public void Init()
        {
            if (Menu.newGame) //if it's a new game we get a new instance of PlayerData
            { 
                playerData = new PlayerData ();
            
                playerData.Reset (); //reset PlayerData and load the game from it
            }
            else //on other platform, we load from the saved xml file
            {
                if (Application.platform == RuntimePlatform.WebGLPlayer)
                {
                    playerData = PlayerData.LoadForWeb ();
                }
                else
                {
                    playerData = PlayerData.Load ();
                }
            }
            Level = playerData.level;
        }

        public void SavaData()
        {
            //everytime we finish a round, automatically save the game
            if (Application.platform == RuntimePlatform.WebGLPlayer) //save method for web player
            {
                playerData.SaveForWeb ();
            }
            else
            {
                playerData.Save ();
            }
        }
    }
}