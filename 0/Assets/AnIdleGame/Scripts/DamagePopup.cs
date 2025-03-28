using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//script set the damagepopup text
public class DamagePopup : MonoBehaviour {

	public void Show (int damage)
	{
		GetComponent<Text> ().text = damage.ToString (); //set the text to the number of the damage you made
	}
}
