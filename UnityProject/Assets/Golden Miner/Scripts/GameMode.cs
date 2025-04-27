using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour {

    public static GameMode Instance;
    [Header("当前游戏分数")]
    public int gameFraction;
    [Header("当前游戏时间")]
    public int time;
    [Header("当前游戏目标分数")]
    public int passFraction;
    [Header("当前游戏关卡数")]
    public int level;
    public Dictionary<string, GameObject> tempLates;        //存储所有的道具预制体模板
    [HideInInspector]
    public string[] objNames;       //所有分数道具的物体名称
    [HideInInspector]
    public int[] fractionData;      //分数数据
    [HideInInspector]
    public string[] propName;       //特殊道具名称
    [HideInInspector]
    public int[] targetFraction;    //关卡的目标分数
    [HideInInspector]
    public float[] scaleData;       //缩放等级

    public int[] gameTimes;         //当前关卡的时间
    [Header("当前关卡生成的所有物体")]
    public List<GameObject> levelObjs;
    public bool isDouble;           //是否进行双倍得分
    public int minFraction = 1500;  //  场景中生成的道具分数总和比目标最小值多1500


    [Header("生成的最小X值")]
    public float minX;
    [Header("生成的最大X值")]
    public float maxX;
    [Header("生成的最小y值")]
    public float minY;
    [Header("生成的最大y值")]
    public float maxY;

    public float gameTime;      //游戏时间
    public bool isPause;        //是否暂停
    [HideInInspector]
    public UIManager uIManager;//UI管理器
    [HideInInspector]
    public Player player;   //玩家

    public Material lineMater;
    private void Awake()
    {
        Instance = this;
    }

    void Start() {
        uIManager = GameObject.FindObjectOfType<UIManager>();
        player = FindObjectOfType<Player>();
        targetFraction = new int[] { 3000, 4000, 5000, 7000, 9000, 12000 ,15000,20000,25000};
        gameTimes = new int[] { 30, 40, 45, 50, 55, 65,65 ,70,70};
        objNames = new string[] { "Diamonds", "gold", "goldTwo", "stoneOne", "stoneTwo" };
        fractionData = new int[] { 1000, 300, 500, 100, 150 };
        propName = new string[] { "explosive", "Potion" };
        scaleData = new float[] { 1.0f, 1.2f, 1.5f, 1.8f, 2.0f };
        tempLates = new Dictionary<string, GameObject>();
        levelObjs = new List<GameObject>();
        InitData();
        SwitchLevel();
        gameTime = gameTimes[level];
        uIManager.UpdateLevelText(level);
        uIManager.UpdateTargetText(targetFraction[level]);
    }

    // Update is called once per frame
    void Update() {
        DrawBoxLine();
        if (!isPause)
        {
            gameTime -= Time.deltaTime;
            uIManager.UpdateTimeText(gameTime);
            if (gameTime < 0)
            {
                isPause = true;
                if (gameFraction > targetFraction[level])
                {
                    uIManager.OpenCloseSwitchPanel();
                }
                else
                {

                    uIManager.OpenDeathPanle();
                }

            }

            if (isCheat)
            {
                if (cheatLine != null)
                {
                    cheatLine.gameObject.SetActive(true);
                }
                CheatFunc();
            }
            else
            {
                if (cheatLine!=null)
                {
                    cheatLine.gameObject.SetActive(false);
                }
            }

        }

    }

    /// <summary>
    /// 切换关卡方法
    /// </summary>
    public void SwitchFunc()
    {
        level++;
        isPause = false;
        isDouble = false;
        uIManager.UpdateLevelText(level);
        SwitchLevel();
        gameTime = gameTimes[level];
        UpdateTargetVaule();
        player.PlayStateRest();
    }


    /// <summary>
    /// 读取并保存所有物体的预制体
    /// </summary>
    public void InitData()
    {
        for (int i = 0; i < objNames.Length; i++)
        {
            GameObject tempObj = ResourcesObj(objNames[i]);
            tempLates.Add(objNames[i], tempObj);
        }

        for (int i = 0; i < propName.Length; i++)
        {
            GameObject tempObj = ResourcesObj(propName[i]);
            tempLates.Add(propName[i], tempObj);
        }
    }

    string path = "Prefabs/";
    public GameObject ResourcesObj(string objName)
    {
        string tempPath = path + objName;
        GameObject tempObj = Resources.Load<GameObject>(tempPath);
        return tempObj;
    }

    /// <summary>
    /// 随机一个道具
    /// </summary>
    /// <returns></returns>
    public GameObject RandomProp(string name)
    {
        GameObject templateObj = tempLates[name];
        Vector3 tempPoint = RandomPos();
        GameObject tempObj = Instantiate(templateObj, tempPoint, Quaternion.identity);
        return tempObj;
    }

    /// <summary>
    /// 随机生成特殊道具
    /// </summary>
    /// <returns></returns>
    public GameObject RandomSpecialProp()
    {
        int index = Random.Range(0, propName.Length);
        string name = propName[index];
        GameObject templateObj = tempLates[name];
        Vector3 tempPoint = RandomPos();
        GameObject tempObj = Instantiate(templateObj, tempPoint, Quaternion.identity);
        PropScript propScript = tempObj.AddComponent<PropScript>();
        if (index == 0)
        {
            propScript.nowType = PropType.Boom;
        }
        else
        {
            propScript.nowType = PropType.Potion;
        }
        return tempObj;
    }

    /// <summary>
    /// 随机道具位置
    /// </summary>
    /// <returns></returns>
    public Vector3 RandomPos()
    {
        float xVaule = Random.Range(minX, maxX);
        float yVaule = Random.Range(minY, maxY);
        Vector3 tempPoint = new Vector3(xVaule, yVaule, 0);
        return tempPoint;
    }
    /// <summary>
    /// 随机道具缩放 返回缩放值，用于计算
    /// </summary>
    public float RandomScale(out int scaleLevel)
    {
        int index = Random.Range(0, scaleData.Length);
        scaleLevel = index+1;
        float scaleVaule = scaleData[index];
        return scaleVaule;
    }

    /// <summary>
    /// 随机道具旋转
    /// </summary>
    /// <returns></returns>
    public Quaternion RandomRotate()
    {
        float angle = Random.Range(0, 360);
        Quaternion tempQuat = Quaternion.AngleAxis(angle, Vector3.forward);
        return tempQuat;
    }

    public void SwitchLevel()
    {
        SceneObjs();
        int creatfraction = 0;      //现在已经生成的分数
        int tempfraction = targetFraction[level];
        tempfraction += minFraction;
        while (creatfraction < tempfraction)
        {
            int objIndex = Random.Range(0, objNames.Length);
            string tempName = objNames[objIndex];
            GameObject tempObj = RandomProp(tempName);
            float tempScale = 1;
            int scaleLevel = 1;
            if (objIndex != 0)//钻石不进行旋转缩放操作
            {
                Quaternion tempQuat = RandomRotate();
                tempObj.transform.rotation = tempQuat;
                tempScale = RandomScale(out scaleLevel);
                tempObj.transform.localScale *= tempScale;
            }
            var tempScript = tempObj.AddComponent<PropScript>();
            tempScript.nowType = PropType.Fraction;
            int fraction = fractionData[objIndex];
            fraction = (int)(fraction * tempScale);
            creatfraction += fraction;
            tempScript.fraction = fraction;
            tempScript.scaleLevel = scaleLevel;
            levelObjs.Add(tempObj);
        }

        int propCount = Random.Range(0, 3);       //场上最多2个道具
        while (propCount > 0)
        {
            propCount--;
            GameObject tempObj = RandomSpecialProp();
            levelObjs.Add(tempObj);
        }

    }

    /// <summary>
    /// 清除场景中的所有物体
    /// </summary>
    public void SceneObjs()
    {
        if (levelObjs.Count > 0)
        {
            for (int i = 0; i < levelObjs.Count; i++)
            {
                Destroy(levelObjs[i]);
            }
        }

        levelObjs.Clear();
    }

    /// <summary>
    /// 分数增加
    /// </summary>
    /// <param name="fraction"></param>
    public void AddFraction(int fraction)
    {
        if (isDouble)
        {
            fraction *= 2;//双倍得分 分数翻倍
        }

        gameFraction += fraction;

        uIManager.UpdateMoneyText(gameFraction);
        uIManager.CreatTipsText(fraction);
    }

    /// <summary>
    /// 计算下一关的目标分数
    /// </summary>
    public void UpdateTargetVaule()
    {
        int targetVaule = 0;
        for (int i=0;i<= level;i++)
        {
            targetVaule += targetFraction[i];
        }
        uIManager.UpdateTargetText(targetVaule);
    }

    public void AddBoomProp()
    {
        uIManager.BoomAddCount();
    }

    public bool UseBoomProp()
    {
        bool isComplte = uIManager.UseBoom();
        return isComplte;
    }

    LineRenderer cheatLine;
    public bool isCheat;        //是否开启作弊

    public void CheatFunc()
    {
        if (cheatLine == null)
        {
            var tempObj = new GameObject();
            cheatLine = tempObj.AddComponent<LineRenderer>();
            cheatLine.startWidth = 0.2f;
            cheatLine.material = lineMater;
            cheatLine.startColor = new Color32(244, 73, 73, 92);
            cheatLine.sortingOrder = 3;
        }

        cheatLine.SetPosition(0, player.transform.position);
        Vector3 targetPoint = player.transform.position + player.transform.up * -1 * 20.0f;
        cheatLine.SetPosition(1, targetPoint);
    }

    public void DrawBoxLine()
    {
        Debug.DrawLine(new Vector3(minX,minY,0), new Vector3(maxX,minY), Color.red);
        Debug.DrawLine(new Vector3(minX, minY, 0), new Vector3(minX, maxY), Color.red);
        Debug.DrawLine(new Vector3(minX, maxY, 0), new Vector3(maxX, maxY), Color.red);
        Debug.DrawLine(new Vector3(maxX, minY, 0), new Vector3(maxX, maxY), Color.red);
    }

    /// <summary>
    /// 炸弹碰到了物体
    /// </summary>
    public void BoomFunc()
    {
        player.RestMoveSpeed();
        if (player.chidrenObj!=null)
        {
            player.isBack = true;
            Destroy(player.chidrenObj.gameObject);
        }
        player.chidrenObj = null;
    }
}
