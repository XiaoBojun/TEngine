using UnityEngine;
using System.Collections;

//script used to created the background
public class Environment : MonoBehaviour {

	public GameObject[] floorTiles;
	public GameObject[] wallTiles;

	private Transform thisTransform;

	void Start ()
	{
		thisTransform = transform;

		for (int i = 0; i < 7; i++)
		{
			for (int j = 0; j < 4; j++) 
			{
				GameObject instance;

				if (j == 3) //instantiate wall tiles on the top row
					instance = Instantiate (wallTiles[Random.Range(0, wallTiles.Length)]);
				else //instantiate floor tiles in other rows
					instance = Instantiate (floorTiles[Random.Range(0, floorTiles.Length)]);

				instance.transform.SetParent (thisTransform);

				instance.transform.localPosition = new Vector3 (i * 64f, j * 64f, 0f); //set the position of each tile, which is 32 unit by 32 unit large
			}
		}
	}
}
