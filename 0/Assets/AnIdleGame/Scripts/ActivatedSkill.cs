using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//Script used to controlled the activated skill
public class ActivatedSkill : Popup {

	//Singleton
	private static ActivatedSkill instance;
	
	public static ActivatedSkill Instance
	{
		get
		{
			if (instance == null)
				instance = GameObject.Find("Player/ActivatedSkill").GetComponent<ActivatedSkill> ();
			
			return instance;
		}
	}

	public Image newSkillAlarm; //when you get a new active skill, this image will pop out
	public Color newSkillAlarmColor; //the color of the image

	private Canvas canvas;
	private Image activatedSkillIcon;

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

		canvas = GetComponent<Canvas> ();

		activatedSkillIcon = GameObject.Find ("Player/ActivatedSkill/Icon/ActivatedSkillIcon").GetComponent<Image> ();
	}

	void Start ()
	{
		activatedSkillIcon.sprite = PlayerStats.Instance.activeSkill.skillIcon; //display the actived skill icon
	}

	public void ChangeActiveSkill () //function called when you click the "Accept" button
	{
		if (PlayerStats.Instance.selectedActiveSkill != null && PlayerStats.Instance.activeSkill != PlayerStats.Instance.selectedActiveSkill) //we change the activated skill only when you selected a active skill and it's not the same as the one currently you are using
		{
			PlayerStats.Instance.activeSkill = PlayerStats.Instance.selectedActiveSkill;
			
			PlayerStats.Instance.UpdateSkillText ();

			activatedSkillIcon.sprite = PlayerStats.Instance.activeSkill.skillIcon;
		}
		
		HUD.Instance.HideAbilityWindow (); //hide the active skill selecting window
	}

	private new IEnumerator Move () //get called by HoverON, which is inherited from Popup. This function will let the popup text follow your cursor.
	{
		while (true)
		{
			Vector2 position;

			RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, Camera.main, out position); //transfer the cursor position on the screen to a position in the canvas (canvas is the private variable we defined above)

			popup.transform.position = canvas.transform.TransformPoint(position); //set the position of the popup text (variable popup is defined in Popup class)
			
			yield return null;
		}
	}

	public void StartNewSkillAlarm () //start new skill alarm
	{
		newSkillAlarm.enabled = true; //enable the image

		StartCoroutine ("NewSkillAlarm");
	}

	public void StopNewSkillAlarm ()
	{
		newSkillAlarm.enabled = false; //disable the image

		StopCoroutine ("NewSkillAlarm");
	}

	private IEnumerator NewSkillAlarm ()
	{
		while (true)
		{
			float t = Mathf.PingPong (Time.time * 1.5f, 1f); //a float will move back and forth between 0 and 1
			
			Color c = newSkillAlarmColor; //create an identical color
			
			c.a = 0; //set the alpha value of the new color to 0
			
			newSkillAlarm.color = Color.Lerp (newSkillAlarmColor, c, t); //loop the color of the alarm image between these two colors
			
			yield return null;
		}
	}
}
