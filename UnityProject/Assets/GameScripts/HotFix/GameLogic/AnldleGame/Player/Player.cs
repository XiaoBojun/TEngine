using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class Player : MonoBehaviour
    {
        private Animator playerAnim; //player animator
        private Cooldown cooldown;

        void Awake()
        {
            playerAnim = GetComponentsInChildren<Animator>()[0];

            cooldown = GetComponentInChildren<Cooldown>();
        }

        public void StartAttack() //function for other scripts to call the attack coroutine
        {
            StartCoroutine("Attack");
        }

        public void StopAttack() //function to stop attack
        {
            StopCoroutine("Attack");

            cooldown.StopCoroutine("WaitForCooldown");
        }

        private IEnumerator Attack() //function to make the attack
        {
            while (AnldleGame.Instance.aliveEnemies.Count > 0) //only attack there is at least one alvie enemy
            {
            yield return cooldown.StartCoroutine("WaitForCooldown"); //wait until cooldown finishes
            
                //get how many targets to attack. If more enemies than the number of max targets, n equals to max target, otherwise the amount of enemies left
                int n = (PlayerStats.Instance.activeSkill.numberOfTargets <= AnldleGame.Instance.aliveEnemies.Count) 
                    ? PlayerStats.Instance.activeSkill.numberOfTargets : AnldleGame.Instance.aliveEnemies.Count;
            
                int[] targets = AnldleGame.Instance.aliveEnemies.ToArray (); //make a copy of alive enemies and turn it into an array
            
                Shuffle (targets); //shuffle the array
            
                for (int i = 0; i < n; i++) //attack the first n enemies
                {
                    AnldleGame.Instance.enemies[targets[i]].TakeDamage (PlayerStats.Instance.activeSkill.AttackDamage);
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