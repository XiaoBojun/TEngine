using System.Collections.Generic;
using GameLogic;
using UnityEngine;
using UnityEngine.UI;
using TEngine;
class TempData
{
    public int id;
}

class TempItem : UILoopItemWidget, IListDataItem<TempData>
{
    #region 脚本工具生成的代码
    private Text _textTemp;
    protected override void ScriptGenerator()
    {
        _textTemp = FindChildComponent<Text>("m_textTemp");
    }
    #endregion

    protected override void OnRefresh()
    {
        base.OnRefresh();
    }

    public void SetItemData(TempData d)
    {
        _textTemp.text = d.id.ToString();
    }
}

namespace GameLogic
{
    [Window(UILayer.UI)]
    class UI_Scrollview : UIWindow
    {
        #region 脚本工具生成的代码
        private ScrollRect _scrollRect;
        private Transform _tfContent;
        private GameObject _itemTemp;
        protected override void ScriptGenerator()
        {
            _scrollRect = FindChildComponent<ScrollRect>("Bg/m_scrollRect");
            _tfContent = FindChild("Bg/m_scrollRect/Viewport/m_tfContent");
            _itemTemp = FindChild("Bg/m_scrollRect/Viewport/m_tfContent/m_itemTemp").gameObject;
        }
        #endregion

        #region 事件
        #endregion

        protected UILoopListWidget<TempItem, TempData> m_loopList;

        protected override void OnRefresh()
        {
            base.OnRefresh();
            m_loopList = CreateWidget<UILoopListWidget<TempItem, TempData>>(_scrollRect.gameObject);
            m_loopList.itemBase = _itemTemp;
        
            List<TempData> datas = new List<TempData>();
            for (int i = 0; i < 100; i++)
            {
                datas.Add(new TempData(){id=i});
            }
            m_loopList.SetDatas(datas);
        }
    }
}