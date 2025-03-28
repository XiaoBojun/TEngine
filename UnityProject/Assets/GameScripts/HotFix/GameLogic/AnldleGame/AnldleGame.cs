using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class AnldleGame:SingletonBehaviour<AnldleGame>
    {
        public List<GameObject>  floorTiles;
        public List<GameObject> wallTiles;
        private Transform thisTransform;
        
        public void OnEnterGame()
        {
            CreateEnv();
            CreatePlayer();
        }

        private void CreatePlayer()
        {
            
        }


        private void CreateEnv()
        {
            floorTiles = new List<GameObject>();
            wallTiles = new List<GameObject>();
            thisTransform = GameObject.Find("Environment").transform;
            for (int i = 1; i <= 9; i++)
            {
                floorTiles.Add(GameModule.Resource.LoadAsset<GameObject>("Dungeon_Floor_" + i));
            }
            for (int i = 1; i <= 4; i++)
            {
                wallTiles.Add(GameModule.Resource.LoadAsset<GameObject>("wall" + i));
            }
           

            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 4; j++) 
                {
                    GameObject instance;

                    if (j == 3) //instantiate wall tiles on the top row
                        instance = Instantiate (wallTiles[Random.Range(0, wallTiles.Count)]);
                    else //instantiate floor tiles in other rows
                        instance = Instantiate (floorTiles[Random.Range(0, floorTiles.Count)]);

                    instance.transform.SetParent (thisTransform);

                    instance.transform.localPosition = new Vector3 (i * 64f, j * 64f, 0f); //set the position of each tile, which is 32 unit by 32 unit large
                }
            }
        }
        
        	public GameObject[] monsterPrefabs; //a list of monster prefabs
	public Enemy enemyPrefab; //the enemy prefab
	public Transform Enemies; //the parent object of enemy
	public int timeLimit= 30; //the time limit of each round
	public Player player; //reference to the Player script
	public float nextRoundDelay=2; //seconds delay before starting next round
	
	[HideInInspector] public List<Enemy> enemies; //a list of enemy scripts
	[HideInInspector] public List<int> aliveEnemies; //a list of alive enmeies
	[HideInInspector] public bool isBattling;
	//[HideInInspector] public ActiveSkill[] activeSkills; //the current using active skill
	//[HideInInspector] public PlayerData playerData; //the selected active skill in active skill selecting window

	private int level; //the monster level
	private int countdown; //the number shows on top-left


	public int Level
	{
		get
		{
			return level;
		}
		set
		{
			level = value;

			//HUD.Instance.levelText.text = "Enemy Level: " + level.ToString (); //update the Level text on bottom-right
		}
	}

	void Awake ()
	{
		// if (instance == null)
		// {
		// 	instance = this;
		// }
		// else
		// {
		// 	if (this != instance)
		// 		Destroy (this.gameObject);
		// }

		//activeSkills = GameObject.Find ("HUD/AbilityWindow/Abilities").GetComponentsInChildren<ActiveSkill> (); //get the list of ActiveSkill scripts

		// if (Menu.newGame) //if it's a new game we get a new instance of PlayerData
		// { 
		// 	playerData = new PlayerData ();
		//
		// 	playerData.Reset (); //reset PlayerData and load the game from it
		// }
		// // else if (Application.i) //if it's not a new game, we need to check which platform we are on. If using web player, we load game from playprefs.
		// // 	playerData = PlayerData.LoadForWeb ();
		// else //on other platform, we load from the saved xml file
		// 	playerData = PlayerData.Load ();
	}

	void Start ()
	{
		enemies = new List<Enemy> ();

		for (int i = 0; i<3; i++) //create 3*3 enemy prefabs
		{
			for (int j=0; j<3; j++)
			{
				Enemy enemy = (Enemy)Instantiate (enemyPrefab);

				enemies.Add(enemy);

				enemy.transform.SetParent (Enemies);

				enemy.transform.localPosition = new Vector2 (i * 50, j * 50);

				enemy.transform.name = "Monster (" + i + ", " + j + ") ";

				enemy.enemyID = i * 3 + j;
			}
		}

		//Level = playerData.level; //load the level

		StartBattle (); //when everything is ready, start the fight
	}

	private void StartBattle ()
	{
		SpawnEnemies ();

		isBattling = true;

		StartCoroutine ("StartCountDown"); //start the time limit count down

		player.StartAttack ();
	}

	public void StopBattle (bool currentLevelCompleted = false)
	{
		player.StopAttack ();

		StopCoroutine ("StartCountDown");

		isBattling = false;

		if (currentLevelCompleted) //if the battle is stopped because all enemies have been killed, we increase the monster level by 1 and check if there is any new skills to unlock
		{
			Level++;

			UnlockSkillCheck (); //unlock new skills if reaching a required level
		}

		//everytime we finish a round, automatically save the game
		// if (Application.isWebPlayer) //save method for web player
		// 	playerData.SaveForWeb ();
		//else
			//playerData.Save ();

		aliveEnemies.Clear (); //remove everything in the alive enemy list
		
		Invoke ("StartBattle", nextRoundDelay); //start a new battle after seconds delay
	}
	
	private IEnumerator StartCountDown ()
	{
		//HUD.Instance.countDownText.text = timeLimit.ToString ();

		countdown = timeLimit;

		while (countdown > 0 && isBattling)
		{
			yield return new WaitForSeconds (1f); //run the loop per second

			countdown--; 

			//HUD.Instance.countDownText.text = countdown.ToString ();
		}

		StopBattle (); //stop the battle if running out of time

		Debug.Log ("time out");
	}

	private void SpawnEnemies ()
	{
		int i = 0;

		foreach (Enemy enemy in enemies)
		{
			enemy.SpawnMonster (level);

			aliveEnemies.Add (i++);
		}
	}

	private void UnlockSkillCheck () //check if there is a new skill can be unlocked
	{
		bool newSkill = false;

		// foreach (ActiveSkill activeSkill in activeSkills)
		// {
		// 	if (level == activeSkill.unlockLevel) //unlock the skill if we beat the required level
		// 	{
		// 		activeSkill.Unlock ();
		//
		// 		newSkill = true; //set to true if a new skill unlocked
		// 	}
		// }

		if (newSkill) //if a new skill gets unlocked, we start the coroutine and remind the player a new skill has been unlocked
			ActivatedSkill.Instance.StartNewSkillAlarm ();
	}

	public void Save () //if you want to make a button and save the game manually, set the click event to this function
	{
		// if (Application.isWebPlayer)
		// 	playerData.SaveForWeb ();
		//else
			//playerData.Save ();
	}
    }
}