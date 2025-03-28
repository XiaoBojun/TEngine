using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO; //for File.Exists

//script used in "Menu" Sence.
public class Menu : MonoBehaviour {

	public Button newGameBtn;
	public Button continueBtn;

	public static bool newGame = true; //if to start a new game

	void Start ()
	{
		// if (Application.isWebPlayer) //on using web player, we check playerprefs.
		// {
		// 	if (PlayerPrefs.HasKey ("PlayerData")) //if there is a saved playerpref, we enable the continue button
		// 		continueBtn.interactable = true;
		// 	else
		// 		continueBtn.interactable = false;
		// }
		//else //on other platform, we check xml file
		{
			if (File.Exists (Application.persistentDataPath + "/PlayerData.xml")) //if there is a saved game, we enable the continue button
				continueBtn.interactable = true;
			else
				continueBtn.interactable = false;
		}

		//Debug.Log (Application.persistentDataPath); //if you don't know where is the presistent data directory, uncomment this line
	}

	public void NewGame () //function called when click new game button
	{
		newGame = true; //set newGame to true

		Application.LoadLevel (1); //load "Game" scene
	}

	public void LoadGame () //function called when click continue button
	{
		newGame = false;

		Application.LoadLevel (1);
	}
}
