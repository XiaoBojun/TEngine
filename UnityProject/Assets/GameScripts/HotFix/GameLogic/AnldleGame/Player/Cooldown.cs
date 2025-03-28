using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic
{
    public class Cooldown : MonoBehaviour {

        private Image cooldown; //the image of visual effect
        private float timer; //when this number is bigger than attackInterval, the character attacks
        private bool isCooldown = false;

        void Awake ()
        {
            cooldown = GameObject.Find("Cooldown").GetComponent<Image> ();
        }

        public IEnumerator WaitForCooldown () 
        {
            cooldown.fillAmount = 1f;

            timer = 0f;

            isCooldown = true;

            while (isCooldown)
            {
                timer += Time.deltaTime;

                // if (timer >= PlayerStats.Instance.activeSkill.AttackInterval)
                // {
                //     isCooldown = false;
                //
                //     cooldown.fillAmount = 0f;
                //
                //     break;
                // }
                //
                // cooldown.fillAmount = 1f - timer / PlayerStats.Instance.activeSkill.AttackInterval;

                yield return null;
            }
        }
    }

}
