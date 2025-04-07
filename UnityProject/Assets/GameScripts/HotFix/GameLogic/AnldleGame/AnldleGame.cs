using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class AnldleGame : SingletonBehaviour<AnldleGame>
    {
        public List<GameObject> floorTiles;
        public List<GameObject> wallTiles;
        private Transform thisTransform;

        //主角
        public Player player; //reference to the Player script

        //敌人
        public List<GameObject> monsterPrefabs;
        public Enemy enemyPrefab; //the enemy prefab
        public Transform Enemies; //the parent object of enemy

        
        
        public int timeLimit = 30; //the time limit of each round

        public float nextRoundDelay = 2; //seconds delay before starting next round

        [HideInInspector]
        public List<Enemy> enemies; //a list of enemy scripts

        [HideInInspector]
        public List<int> aliveEnemies; //a list of alive enmeies

        [HideInInspector]
        public bool isBattling;

        [HideInInspector]
        public ActiveSkill[] activeSkills; //the current using active skill




        private int countdown; //the number shows on top-left
        public async void OnEnterGame()
        {
            //data
            AnldleGame_Data.Instance.Init();
            
            //view
            CreateEnv();
            await CreatePlayer();
            await CreateEnemy();
            GameModule.UI.ShowUI<UI_AbilityWindow>();
            //
            StartBattle(); //when everything is ready, start the fight
        }

        private async UniTask CreateEnemy()
        {
            Enemies = GameObject.Find("Enemies").transform;
            aliveEnemies = new List<int>();
            monsterPrefabs = new List<GameObject>();
            for (int i = 1; i < 7; i++)
            {
                var monster = await GameModule.Resource.LoadAssetAsync<GameObject>("Monster" + i);
                monsterPrefabs.Add(monster);
            }

            var go = await GameModule.Resource.LoadAssetAsync<GameObject>("Enemy");
            enemyPrefab = go.GetComponent<Enemy>();

            enemies = new List<Enemy>();

            for (int i = 0; i < 3; i++) //create 3*3 enemy prefabs
            {
                for (int j = 0; j < 3; j++)
                {
                    Enemy enemy = (Enemy)Instantiate(enemyPrefab);

                    enemies.Add(enemy);

                    enemy.transform.SetParent(Enemies);

                    enemy.transform.localPosition = new Vector2(i * 50, j * 50);

                    enemy.transform.name = "Monster (" + i + ", " + j + ") ";

                    enemy.enemyID = i * 3 + j;
                }
            }
        }

        private async UniTask CreatePlayer()
        {
            var go = await GameModule.Resource.LoadGameObjectAsync("Player");
            player = go.GetComponent<Player>();
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
                        instance = Instantiate(wallTiles[Random.Range(0, wallTiles.Count)]);
                    else //instantiate floor tiles in other rows
                        instance = Instantiate(floorTiles[Random.Range(0, floorTiles.Count)]);

                    instance.transform.SetParent(thisTransform);

                    instance.transform.localPosition = new Vector3(i * 64f, j * 64f, 0f); //set the position of each tile, which is 32 unit by 32 unit large
                }
            }
        }


        
        private void StartBattle()
        {
            SpawnEnemies();

            isBattling = true;

            //StartCoroutine("StartCountDown"); //start the time limit count down

            player.StartAttack();
        }

        public void StopBattle(bool currentLevelCompleted = false)
        {
            player.StopAttack();

            StopCoroutine("StartCountDown");

            isBattling = false;

            if (currentLevelCompleted) //if the battle is stopped because all enemies have been killed, we increase the monster level by 1 and check if there is any new skills to unlock
            {
                AnldleGame_Data.Instance.LevelAdd();

                UnlockSkillCheck(); //unlock new skills if reaching a required level
            }

            AnldleGame_Data.Instance.SavaData();

            aliveEnemies.Clear(); //remove everything in the alive enemy list

            Invoke("StartBattle", nextRoundDelay); //start a new battle after seconds delay
        }

        private IEnumerator StartCountDown()
        {
            GameEvent.Send(IUI_HUD_Event.Update_CountDown,timeLimit);
            countdown = timeLimit;

            while (countdown > 0 && isBattling)
            {
                yield return new WaitForSeconds(1f); //run the loop per second

                countdown--;
                
                GameEvent.Send(IUI_HUD_Event.Update_CountDown,countdown);
            }

            StopBattle(); //stop the battle if running out of time

            Debug.Log("time out");
        }

        private void SpawnEnemies()
        {
            int i = 0;
            foreach (Enemy enemy in enemies)
            {
                enemy.SpawnMonster(AnldleGame_Data.Instance.Level);
                aliveEnemies.Add(i++);
            }
        }

        private void UnlockSkillCheck() //check if there is a new skill can be unlocked
        {
            bool newSkill = false;
            foreach (ActiveSkill activeSkill in activeSkills)
            {
            	if (AnldleGame_Data.Instance.Level == activeSkill.unlockLevel) //unlock the skill if we beat the required level
            	{
            		activeSkill.Unlock ();
            
            		newSkill = true; //set to true if a new skill unlocked
            	}
            }
            if (newSkill) //if a new skill gets unlocked, we start the coroutine and remind the player a new skill has been unlocked
                ActivatedSkill.Instance.StartNewSkillAlarm();
        }
    }
}