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
    }
}