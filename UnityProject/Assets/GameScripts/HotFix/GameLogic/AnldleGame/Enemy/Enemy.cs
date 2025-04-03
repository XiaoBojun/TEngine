using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class Enemy : MonoBehaviour
    {
        public GameObject damagePopup; //a reference to the damagePopup prefab
        public Transform PopupSpawnPoint; //the parent object
        public int enemyID;

        private int maxHealth; //the max health
        private int loot; //how much money the player will get after killing this enemy
        private int health; //the current health
        //private HealthBar healthBar; //reference to the healthBar script on this enemy
        private Animator anim;
        private ParticleSystem deathParticle; //particle will play after death
        private GameObject canvas; //the canvas used to show health bar and damage popup text

        void Awake()
        {
            //healthBar = GetComponentInChildren<HealthBar>();

            deathParticle = GetComponentInChildren<ParticleSystem>();

            canvas = GetComponentInChildren<Canvas>().gameObject;
        }

        public void TakeDamage(int damage) //function get called if the player attacks this enemy
        {
            if (!anim) //see if ther is no monster prefab under this enemy object. this should never happen
            {
                Debug.Log("Need to reset monster");

                return;
            }

            health -= damage;

            if (health <= 0) //if the current health is equal to or smaller than 0, this enmey should die
            {
                health = 0; //change the value to 0, make sure it's not negative. we will used this value to calculate health ratio

                Destroy(anim.gameObject);

                deathParticle.Play(); //play death partivle

                canvas.SetActive(false); //hide the canvas

                //PlayerStats.Instance.Money += loot; //give the player the reward

                //GameManager.Instance.aliveEnemies.Remove(enemyID); //remove this enmey from the enemy list
            }

            float healthRatio = (float)health / maxHealth; //calculate the health ratio, this value will be used by HealthBar script. 

            GameObject popupInstance = Instantiate(damagePopup); //create the damage popup text

            popupInstance.transform.SetParent(PopupSpawnPoint, false);

            //popupInstance.GetComponent<DamagePopup>().Show(damage); //call the function in DamagePupup script

            Destroy(popupInstance, 1f); //destory the damage popup after 1 second

            anim.SetTrigger("Damage"); //trigger the animation when the enemy gets attacked

            //healthBar.Change(healthRatio); //call the function in HealthBar script to update the health bar
        }

        public void SpawnMonster(int level) //function used to spawn monster prefab
        {
            if (anim) //if this enmey doesn't get killed in the last round, we destroy the old one
                Destroy(anim.gameObject); //destroy the old monster prefab

            maxHealth = (int)(100 * Mathf.Pow(1.2f, level)); //formula to determine the health of emeny

            loot = level; //formula to determine the amount of the reward

            health = maxHealth; //set current health to full

            //healthBar.Reset(); //reset the health bar

            // GameObject monster =
            //     (GameObject)Instantiate(GameManager.Instance.monsterPrefabs[Random.Range(0, GameManager.Instance.monsterPrefabs.Length)]); //randomly create a monster prefab
            //
            // monster.transform.SetParent(transform);
            //
            // monster.transform.localPosition = Vector2.zero;

            canvas.SetActive(true); //show canvas again

            //anim = monster.GetComponent<Animator>(); //set the reference to the animator on the newly created monster prefab
        }
    }
}