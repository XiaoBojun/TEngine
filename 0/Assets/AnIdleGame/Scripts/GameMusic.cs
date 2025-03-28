using UnityEngine;
using System.Collections;

//script used to keep music playing between two sences
public class GameMusic : MonoBehaviour {
	
	private static GameMusic instance;

	public static GameMusic Instance
	{
		get
		{
			if (instance == null)
				instance = GameObject.Find("GameMusic").GetComponent<GameMusic> ();
			
			return instance;
		}
	}

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

		DontDestroyOnLoad (this.gameObject);
	}
}
