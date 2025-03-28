using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//script used to unpdate the skill level and cost texts
public class SkillTextInfo : Popup {

	public Text skillLevelText;
	public Text upgradeCostText;

	public void UpdateSkillText (int skillLevel, int upgradeCost)
	{
		skillLevelText.text = "Lv. " + skillLevel.ToString ();

		upgradeCostText.text = "Cost: " + upgradeCost.ToString ();
	}
}
