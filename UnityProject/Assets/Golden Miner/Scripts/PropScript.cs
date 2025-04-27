using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PropType
{
    None,
    /// <summary>
    /// 分数类型道具
    /// </summary>
    Fraction,
    /// <summary>
    /// 炸弹
    /// </summary>
    Boom,
    /// <summary>
    /// 双倍药剂
    /// </summary>
    Potion,

}

public class PropScript : MonoBehaviour {

    public int fraction;
    public PropType nowType;
    public int scaleLevel=1;  //当前道具的缩放道具  默认为1  用于计算玩家钩住时的速度
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UseProp()
    {

        switch (nowType)
        {
            case PropType.Fraction:
                GameMode.Instance.AddFraction(fraction);
                break;
            case PropType.Potion:
                GameMode.Instance.isDouble = true;
                break;
            case PropType.Boom:
                GameMode.Instance.AddBoomProp();
                break;
        }
    }
}
