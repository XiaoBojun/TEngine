using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    public Text timeText;
    public Text nowMoneyText;
    public Text targetMoneyText;
    public Text levelText;

    public GameObject startPanel;
    public GameObject tipsPanel;
    public GameObject endPanel;
    public GameObject setPanel;
    public GameObject switchPanel;

    public Button startBtn;
    public Button switchBtn;

    public Transform fractionPoint;
    public Transform boomGird;

    public Dictionary<string, GameObject> uIPrefabs;    //存储UI的预制体
    public List<GameObject> boomObjs;
    public string[] prefabNames;
    void Awake() {
        startPanel = transform.Find("StartPanel").gameObject;
        tipsPanel = transform.Find("TipsPanel").gameObject;
        switchPanel = transform.Find("switchPanel").gameObject;
        switchBtn = transform.Find("switchPanel/switchBtn").GetComponent<Button>();
        startBtn = transform.Find("StartPanel/startBtn").GetComponent<Button>();
        timeText = transform.Find("TipsPanel/timeText").GetComponent<Text>();
        nowMoneyText = transform.Find("TipsPanel/moneyText").GetComponent<Text>();
        targetMoneyText = transform.Find("TipsPanel/targetMoney").GetComponent<Text>();
        levelText = transform.Find("TipsPanel/levelText").GetComponent<Text>();
        fractionPoint = transform.Find("fractionPoint");
        boomGird = transform.Find("TipsPanel/boomGird");
        UpdateMoneyText(0);
        tipsPanel.SetActive(false);
        startBtn.onClick.AddListener(StartClick);
        uIPrefabs = new Dictionary<string, GameObject>();
        prefabNames = new string[] { "fractionText", "boomUI" };
        boomObjs = new List<GameObject>();
        switchBtn.onClick.AddListener(SwitchClick);
        switchPanel.SetActive(false);
    }

    private void Start()
    {
        GameMode.Instance.isPause = true;
        InitData();
        InitPanelData();
    }
    // Update is called once per frame
    void Update() {

    }

    public void InitData()
    {
        for (int i = 0; i < prefabNames.Length; i++)
        {
            GameObject tempObj = GameMode.Instance.ResourcesObj(prefabNames[i]);
            uIPrefabs.Add(prefabNames[i], tempObj);
        }
    }

    /// <summary>
    /// 初始化panel数据
    /// </summary>
    public void InitPanelData()
    {
        endPanel = transform.Find("endPanel").gameObject;
        setPanel = transform.Find("setPanel").gameObject;
        Button cheatBtn = setPanel.transform.Find("cheatBtn").GetComponent<Button>();
        cheatBtn.onClick.AddListener(CheatClick);
        BtnAddClick(endPanel.transform);
        BtnAddClick(setPanel.transform);
        endPanel.SetActive(false);
        setPanel.SetActive(false);
    }

    public void BtnAddClick(Transform trans)
    {
        Button restBtn = trans.Find("restBtn").GetComponent<Button>();
        Button quitBtn = trans.Find("quitBtn").GetComponent<Button>();
        restBtn.onClick.AddListener(RestClick);
        quitBtn.onClick.AddListener(QuitClick);
    }

    public void UpdateTimeText(float time)
    {
        timeText.text = "时间：" + ((int)time).ToString();
    }

    public void UpdateMoneyText(int fraction)
    {
        nowMoneyText.text = "当前分数：" + fraction.ToString();
    }

    public void UpdateLevelText(int level)
    {

        levelText.text = "关卡：" + (level + 1).ToString();
    }

    public void UpdateTargetText(int targetFraction)
    {
        targetMoneyText.text = "当前目标：" + targetFraction.ToString();
    }

    /// <summary>
    /// 生成分数提示文本
    /// </summary>
    public void CreatTipsText(int fraction)
    {
        GameObject templateObj = uIPrefabs[prefabNames[0]];
        Vector3 creatPoint = Camera.main.WorldToScreenPoint(GameMode.Instance.player.transform.position);
        GameObject tempObj = Instantiate(templateObj, creatPoint, Quaternion.identity);
        Text tempText = tempObj.transform.GetComponent<Text>();
        tempText.text = fraction.ToString();
        tempObj.transform.SetParent(transform);
        StartCoroutine(TipsMove(tempObj.transform));

    }

    IEnumerator TipsMove(Transform targetTrans)
    {
        float scaleVaule = 0.1f;
        targetTrans.localScale = new Vector3(scaleVaule, scaleVaule);
        Vector3 startPoint = targetTrans.position;
        yield return null;
        for (int i = 0; i < 9; i++)
        {
            scaleVaule += 0.1f;
            targetTrans.localScale = new Vector3(scaleVaule, scaleVaule);
            targetTrans.position = Vector3.Lerp(startPoint, fractionPoint.position, scaleVaule);
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        Destroy(targetTrans.gameObject);
        yield break;
    }

    public void StartClick()
    {
        GameMode.Instance.isPause = false;
        startPanel.SetActive(false);
        tipsPanel.SetActive(true);
    }

    /// <summary>
    /// 开启与关闭切换面版
    /// </summary>
    public void OpenCloseSwitchPanel()
    {
        bool isOpen = switchPanel.activeSelf;
        if (isOpen)
        {
            switchPanel.SetActive(false);
        }
        else
        {
            switchPanel.SetActive(true);
        }

    }

    public void SwitchClick()
    {
        GameMode.Instance.SwitchFunc();
        OpenCloseSwitchPanel();
    }

    /// <summary>
    /// 打开死亡面版
    /// </summary>
    public void OpenDeathPanle()
    {
        endPanel.SetActive(true);
    }

    /// <summary>
    /// 打开设置面版
    /// </summary>
    public void OpenSetPanle()
    {
        bool isOpen = setPanel.activeSelf;
        if (isOpen)
        {
            setPanel.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
            setPanel.SetActive(true);
        }

    }

    /// <summary>
    /// 重置按钮的响应事件
    /// </summary>
    public void RestClick()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// 退出按钮的响应事件
    /// </summary>
    public void QuitClick()
    {
        Application.Quit();
    }

    /// <summary>
    /// 作弊模式的响应事件
    /// </summary>
    public void CheatClick()
    {
        bool isEnable = GameMode.Instance.isCheat;
        if (isEnable)
        {
            GameMode.Instance.isCheat = false;
        }
        else
        {
            GameMode.Instance.isCheat = true;
        }
    }

    /// <summary>
    /// 炸弹添加UI
    /// </summary>
    public void BoomAddCount()
    {
        GameObject tempLate = uIPrefabs[prefabNames[1]];
        GameObject tempObj = Instantiate(tempLate, boomGird);
        boomObjs.Add(tempObj);
    }

    /// <summary>
    /// 使用炸弹
    /// </summary>
    public bool UseBoom()
    {
        if (boomObjs.Count>0)
        {
            GameObject boomUI = boomObjs[(boomObjs.Count - 1)];
            boomObjs.RemoveAt((boomObjs.Count - 1));
            Destroy(boomUI);
            return true;
        }
        return false;
    }
}
