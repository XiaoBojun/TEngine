using Cysharp.Threading.Tasks;
using TEngine;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameLogic
{
    public class Player : SingletonBehaviour<Player>
    {
        private Animator playerAnim;
        
        void Awake()
        {
            playerAnim = GetComponentsInChildren<Animator>()[0];
            transform.position = new Vector3(-125, 40, 0);
        }

        public void StartAttack() //function for other scripts to call the attack coroutine
        {
            //StartCoroutine("Attack");
            //Attack().Forget();
        }

        public void StopAttack() //function to stop attack
        {
            //StopCoroutine("Attack");

            //cooldown.StopCoroutine("WaitForCooldown");
        }

        private async UniTaskVoid Attack()
        {
            //敌人存在
            while (AnldleGame.Instance.aliveEnemies.Count > 0)
            {
                GameEvent.Get<IUI_ActivatedSkill>().Action_WaitForCooldown();
                //计算冷却
                await UniTask.WaitUntil(()=>AnldleGame_Data.Instance.isCoolDown);
                int n = (AnldleGame_Data.Instance.activeSkill_numberOfTargets <= AnldleGame.Instance.aliveEnemies.Count) ?
                              AnldleGame_Data.Instance.activeSkill_numberOfTargets : AnldleGame.Instance.aliveEnemies.Count;
                int[] targets = AnldleGame.Instance.aliveEnemies.ToArray ();
                Shuffle (targets); //shuffle the array
                
                for (int i = 0; i < n; i++) //attack the first n enemies
                {
                    AnldleGame.Instance.enemies[targets[i]].TakeDamage (AnldleGame_Data.Instance.AttackDamage);
                }
                
                playerAnim.SetTrigger ("Attack"); //trigger the player animation
            }
            if (AnldleGame.Instance.isBattling) //if we kill all enemies in time
            {
                Debug.Log ("All Killed");
            
                AnldleGame.Instance.StopBattle (true); //we stop the battle with levelCompleted set to true
            }
        }
        
        private static void Shuffle<T>(T[] array) //function used to shuffle an array
        {
            int n = array.Length;

            for (int i = 0; i < n; i++)
            {
                int index = i + Random.Range(0, n - i);

                T t = array[index];

                array[index] = array[i];

                array[i] = t;
            }
        }
    }
}