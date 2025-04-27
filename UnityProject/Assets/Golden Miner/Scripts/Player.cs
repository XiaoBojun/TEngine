using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotaDir
{
    left,
    right,
}

public class Player : MonoBehaviour {

    public Transform startTrans;    //起始点
    public RotaDir nowDir;
    public float angleSpeed;
    public float moveSpeed;
    LineRenderer lineRenderer;
    public bool isFire;
    public Vector3 playStartPoint;

    public bool isBack; //玩家是否返回

    public PropScript chidrenObj;       //现在被抓到的物体

    public GameObject boomObj;          //炸弹的预制体
    private float startSpeed;
    void Start() {
        isFire = false;
        isBack = false;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        startSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update() {

        if (GameMode.Instance.isPause)
        {
            return;
        }


        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!isFire)
            {
                isFire = true;
                playStartPoint = transform.position;
            }

        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (chidrenObj!=null)
            {
                Debug.Log("炸弹生成");
                BoomFire();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameMode.Instance.uIManager.OpenSetPanle();
        }

        if (isFire && !isBack)
        {
            PlayMoveForward();
            CheckExceed();
        }
        else if (isFire && isBack)
        {
            PlayBackMove();
            CheckMoveToStart();
        }


        if (!isFire)
        {
            PlayRotate();
        }

        UpdataLine();
    }

    public void PlayRotate()
    {

        float rightAngle = Vector3.Angle(transform.up * -1, Vector3.right);


        if (nowDir == RotaDir.left)
        {
            if (rightAngle < 170)
            {
                transform.RotateAround(startTrans.position, Vector3.forward, angleSpeed * Time.deltaTime);
            }
            else
            {
                nowDir = RotaDir.right;
            }

        }
        else
        {
            if (rightAngle > 10)
            {
                transform.RotateAround(startTrans.position, Vector3.forward, -angleSpeed * Time.deltaTime);
            }
            else
            {
                nowDir = RotaDir.left;
            }

        }
    }

    public void UpdataLine()
    {
        lineRenderer.SetPosition(0, startTrans.position);
        lineRenderer.SetPosition(1, transform.position);
    }

    /// <summary>
    /// 发射炸弹
    /// </summary>
    public void BoomFire()
    {
        bool isOK =  GameMode.Instance.UseBoomProp();
        if (isOK)
        {
            GameObject tempObj = Instantiate(boomObj, startTrans.position, Quaternion.identity);
            tempObj.AddComponent<BoomCollison>();
            tempObj.transform.up = transform.up;
        }
    }
    /// <summary>
    /// 玩家返回移动
    /// </summary>
    public void PlayBackMove()
    {
        transform.position += transform.up * moveSpeed * Time.deltaTime;
    }

    /// <summary>
    /// 玩家前进移动
    /// </summary>
    public void PlayMoveForward()
    {
        transform.position += transform.up * -1 * moveSpeed * Time.deltaTime;
    }

    /// <summary>
    /// 检测玩家是否超出边界
    /// </summary>
    public void CheckExceed()
    {
        float x = transform.position.x;
        float y = transform.position.y;
        if (y < GameMode.Instance.minY || x > GameMode.Instance.maxX || x < GameMode.Instance.minX)
        {
            isBack = true;  //超出边界开始返回
        }
    }

    /// <summary>
    /// 检测是否回到原点
    /// </summary>
    public void CheckMoveToStart()
    {
        float distance = Vector3.Distance(transform.position, playStartPoint);
        if (distance < 0.2f)
        {
            transform.position = playStartPoint;
            if (chidrenObj != null)
            {
                chidrenObj.UseProp();
                Destroy(chidrenObj.gameObject);
                chidrenObj = null;
            }
            isFire = false;
            isBack = false;
            RestMoveSpeed();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (chidrenObj != null)
        {
            return;
        }
        PropScript propScript = collision.gameObject.GetComponent<PropScript>();
        if (propScript != null)
        {
            float tempDistance = Vector3.Distance(transform.position, propScript.transform.position);
            propScript.transform.position = transform.position + transform.up * -1 * tempDistance;
            propScript.transform.SetParent(transform);
            chidrenObj = propScript;
            ComputeSpeed(propScript.scaleLevel);
            isBack = true;
        }
        else
        {
            Debug.Log("没有脚本");
        }
    }

    /// <summary>
    /// 计算玩家新的速度
    /// </summary>
    public void ComputeSpeed(int scaleLevel)
    {
        moveSpeed = moveSpeed - moveSpeed * 0.15f * scaleLevel;
    }

    /// <summary>
    /// 玩家速度重置
    /// </summary>
    public void RestMoveSpeed()
    {
        moveSpeed = startSpeed;
    }

    /// <summary>
    /// 玩家状态重置
    /// </summary>
    public void PlayStateRest()
    {
        if (isFire)
        {
            transform.position = playStartPoint;
        }
        if (chidrenObj != null)
        {
            chidrenObj.transform.SetParent(null);
        }
        isFire = false;
        isBack = false;
        RestMoveSpeed();
    }
}
