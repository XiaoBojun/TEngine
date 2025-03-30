using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
	public class PlayerStats : MonoBehaviour
	{

		//Singleton
		private static PlayerStats instance;

		public static PlayerStats Instance
		{
			get
			{
				if (instance == null)
					instance = GameObject.Find("Player").GetComponent<PlayerStats>();

				return instance;
			}
		}

		//public SkillTextInfo damageText;
		//public SkillTextInfo speedText;

		// [HideInInspector]
		// public ActiveSkill activeSkill; //reference to the current using active skill
		//
		// [HideInInspector]
		// public ActiveSkill selectedActiveSkill; //used when you select a skill in active skill selecting window

		
		void Start()
		{
			// foreach (ActiveSkill activeSkill in GameManager.Instance.activeSkills) //load the activeSkill which has the saved ID
			// {
			// 	if (activeSkill.ID == GameManager.Instance.playerData.activatedSkillID)
			// 	{
			// 		this.activeSkill = selectedActiveSkill = activeSkill;
			//
			// 		break;
			// 	}
			// }
			//
			// UpdateSkillText(); //call the function to update the text with corresponding damage and speed level to the actived skill
			//
			// Money = GameManager.Instance.playerData.money; //load the amount of money
			//
			// TimeSpan timeSinceLastPlay = DateTime.Now - GameManager.Instance.playerData.quitTime; //calculate the time difference between last play and now
			//
			// if (timeSinceLastPlay > TimeSpan.Zero) //if the difference is not negative
			// 	Money += (int)(timeSinceLastPlay.TotalHours * Mathf.Pow(GameManager.Instance.playerData.level, 2f)); //gain money according to the time difference
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
	}
}
