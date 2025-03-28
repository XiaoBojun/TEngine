using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//script controls the HUD object
public class HUD : MonoBehaviour {

	//Singleton
	private static HUD instance;
	
	public static HUD Instance
	{
		get
		{
			if (instance == null)
				instance = GameObject.Find("HUD").GetComponent<HUD> ();
			
			return instance;
		}
	}

	public Text moneyText; //text to display the money the player has
	public Text levelText; //text to display the current monster level
	public Text countDownText; //text to show how much time left in this round
	public Color highlightColor; //color when a skill is selected

	private CanvasGroup abilityWindow; //the active skill selecting window

	void Awake ()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			if (this != instance)
				Destroy (this.gameObject);
		}

		abilityWindow = GetComponentInChildren<CanvasGroup> ();
	}

	void Start () //used to position active skills to a 2 by 4 layout. Delete this block if you wish to position each skill manually. You also can change the code to form a different layout or use more or less skills
	{
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				RectTransform rectTransform = GameManager.Instance.activeSkills[i * 4 + j].GetComponent<RectTransform> ();

				rectTransform.anchorMin = new Vector2 (0.15f + 0.2f * j, 0.5f - 0.3f * i);

				rectTransform.anchorMax = new Vector2 (0.25f + 0.2f * j, 0.8f - 0.3f * i);

				rectTransform.offsetMin = rectTransform.offsetMax = Vector2.zero;
			}
		}
	}

	public void ShowAbilityWindow () //show the active skill selecting window
	{
		PlayerStats.Instance.activeSkill.SetColor (highlightColor); //highlight the skill currently using

		abilityWindow.alpha = 1f;
		
		abilityWindow.interactable = true;

		abilityWindow.blocksRaycasts = true;
	}

	public void HideAbilityWindow () //hide the window
	{
		PlayerStats.Instance.selectedActiveSkill.SetColor (Color.white);

		PlayerStats.Instance.selectedActiveSkill = PlayerStats.Instance.activeSkill;

		abilityWindow.alpha = 0f;

		abilityWindow.interactable = false;

		abilityWindow.blocksRaycasts = false;

		ActivatedSkill.Instance.StopNewSkillAlarm (); //stop the new skill alarm
	}
}
