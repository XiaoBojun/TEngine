using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//script for each active skill
public class ActiveSkill : MonoBehaviour {

	// public int ID; //the ID of the active skill
	// public int baseAttackDamage; //the initial damage of the skill
	// public float baseAttackInterval; //the initial cooldown period 
	// public int numberOfTargets; //how many targets the skill can attack each time
	// public Sprite skillIcon; //skill icon sprite
	// public bool isStartingSkill; //if it's a starting skill
	// public int unlockLevel; //monster level required to unlock the skill
	// public string description; //the description of the skill, 
	// public Text textPopup; //the popup text to show the description
	//
	// private Image skillImage; //the image to show the skill icon
	// private int damageLevel;
	// private int damageUpgradeCost; //the amount of money needed to upgrade
	// private int attackDamage; //the current damage calculated based on damage level
	// private float attackInterval; //the current cooldown period calculated based on speed level
	// private int speedLevel;
	// private int speedUpgradeCost; //the amount of money needed to upgrade
	// private bool unlocked = false; //skill cannot be used until unlocked

	public int ID; // 技能的唯一标识符
	public int baseAttackDamage; // 技能的基础伤害值
	public float baseAttackInterval; // 技能的基础冷却时间
	public int numberOfTargets; // 技能每次可以攻击的目标数量
	public Sprite skillIcon; // 技能图标的精灵
	public bool isStartingSkill; // 标记该技能是否为起始技能
	public int unlockLevel; // 解锁该技能所需的怪物等级
	public string description; // 技能的描述信息
	public Text textPopup; // 用于显示技能描述的弹出文本

	private Image skillImage; // 用于展示技能图标的图像组件
	private int damageLevel; // 技能伤害等级
	private int damageUpgradeCost; // 提升伤害等级所需的费用
	private int attackDamage; // 当前基于伤害等级计算的攻击伤害
	private float attackInterval; // 当前基于速度等级计算的冷却时间
	private int speedLevel; // 技能速度等级
	private int speedUpgradeCost; // 提升速度等级所需的费用
	private bool unlocked = false; // 技能解锁状态，未解锁时无法使用

	void Awake ()
	{
		DamageLevel = GameManager.Instance.playerData.skills[ID].damageLevel; //load damage level from PlayerData

		SpeedLevel = GameManager.Instance.playerData.skills[ID].speedLevel; //load speed level from PlayerData

		if (description == "") //if the description is omitted, set up the default description
		{
			if (numberOfTargets == 1)
			{
				description = "This skill can attack 1 enemy.";
			}
			else
			{
				description = "This skill can attack up to " + numberOfTargets.ToString () + " enemies.";
			}
		}

		skillImage = GetComponent<Image> ();

		skillImage.sprite = skillIcon; //set up the image to show the skill icon

		if (isStartingSkill) //unlock the skill if it's a starting skill
			Unlock ();

		if (GameManager.Instance.playerData.skills [ID].unlocked) //according to the saved game file, unlock the skill if it has been unlocked
			Unlock ();
	}

	public int DamageLevel
	{
		get
		{
			return damageLevel;
		}
		set
		{
			damageLevel = value;
			
			damageUpgradeCost = (int)(10 * Mathf.Pow (1.1f, damageLevel)); //formula to calculate the upgrade cost
			
			attackDamage = (int)(baseAttackDamage * Mathf.Pow (1.05f, damageLevel)); //formula to calculate the damage
		}
	}

	public int AttackDamage
	{
		get {return attackDamage;}
	}

	public int DamageUpgradeCost
	{
		get	{return damageUpgradeCost;}
	}

	public int SpeedLevel
	{
		get
		{
			return speedLevel;
		}
		set
		{
			speedLevel = value;
			
			speedUpgradeCost = (int)(10 * Mathf.Pow (1.1f, speedLevel)); //formula to calculate the upgrade cost
			
			attackInterval = baseAttackInterval * Mathf.Pow(0.95f, speedLevel); //formula to calculate the cooldown period
		}
	}

	public float AttackInterval
	{
		get {return attackInterval;}
	}

	public int SpeedUpgradeCost
	{
		get	{return speedUpgradeCost;}
	}

	public bool Unlocked
	{
		get {return unlocked;}
	}

	public void Unlock () //unlock this active skill
	{
		if (!unlocked)
		{
			unlocked = true;

			Color color = skillImage.color; //this and following two lines will change the alpha value to 255

			color.a = 255; 

			skillImage.color = color;
		}
	}

	public void Select () //function called when you select this skill in the avtive skill selecting window
	{
		if (unlocked) //only unlocked skills can be selected
		{
			PlayerStats.Instance.selectedActiveSkill.SetColor (Color.white); //change the color of the former selected skill to normal

			skillImage.color = HUD.Instance.highlightColor; //change the color of this skill when it's selected

			PlayerStats.Instance.selectedActiveSkill = this; //update the selectedActiveSkill varialbe in PlayerStats script
		}
	}

	public void HoverOn () //when we hover over the skill icon, display the popup text
	{
		textPopup.text = description;

		if (!unlocked) //if the skill is unlocked, we add the info to tell player when it's going to be unlocked
		{
			textPopup.text += "\nUnlock after finishing Level: " + unlockLevel.ToString ();
		}

		StartCoroutine ("Move"); //let the popup follow the cursor

		textPopup.gameObject.SetActive (true); //enable the popup
	}

	public void HoverOff () //ater the cursor left the icon, we hide the popup text 
	{
		textPopup.gameObject.SetActive (false); //enable the popup

		StopCoroutine ("Move"); //stop the following
	}

	private IEnumerator Move ()
	{
		while (true)
		{
			textPopup.transform.position = Input.mousePosition;

			yield return null;
		}
	}

	public void SetColor (Color color) //this function allows us to change the color of the icon image in other script
	{
		skillImage.color = color;
	}
}
