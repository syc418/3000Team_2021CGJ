using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr
{
    public static UIMgr Inst => inst;
    private static UIMgr inst = new UIMgr();

    public Transform UIParent => uiParent;
    private Transform uiParent;

    public Camera MainCam => mainCam;
    private Camera mainCam;

    public Camera UICam => uiCam;
    private Camera uiCam;

    public Vector2 UIPosMin => uiPosMin;
    private Vector2 uiPosMin;

    public Vector2 UIPosMax => uiPosMax;
    private Vector2 uiPosMax;

    private Canvas canvas;

    private Dictionary<string, UIPanelBase> panelDict = new Dictionary<string, UIPanelBase>();

    public UIMgr()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        uiParent = GameObject.Find("UI Parent").transform;
        uiParent.SetParent(canvas.transform);
        GameObject.DontDestroyOnLoad(uiParent.gameObject);

        mainCam = Camera.main;
        uiCam = GameObject.Find("UI Camera").GetComponent<Camera>();

        uiPosMin = uiCam.ViewportToWorldPoint(Vector3.zero);
        uiPosMax = uiCam.ViewportToWorldPoint(Vector3.one);
    }

    /// <summary>
    /// 打开UI,如果没有创建则创建.
    /// </summary>
    public T OpenUIPanel<T>() where T : UIPanelBase
    {
        string panelName = typeof(T).Name;
        UIPanelBase panel;
        if (!panelDict.TryGetValue(panelName, out panel))
        {
            panel = FactoryPanel<T>();
            panelDict[panelName] = panel;
        }

        panel.Show();
        return panel as T;
    }

    /// <summary>
    /// 获取UI,如果没有就返回Null.
    /// </summary>
    public T GetUIPanel<T>() where T : UIPanelBase
    {
        string panelName = typeof(T).Name;
        UIPanelBase panel;
        panelDict.TryGetValue(panelName, out panel);
        return panel as T;
    }

    /// <summary>
    /// 隐藏UI.
    /// </summary>
    public void HideUIPanel<T>() where T : UIPanelBase
    {
        string panelName = typeof(T).Name;
        UIPanelBase panel;
        if (panelDict.TryGetValue(panelName, out panel))
        {
            panel.Hide();
        }
    }

    /// <summary>
    /// 独立创建UI
    /// 销毁需调用此类中的Destroy_CreateUIPanel函数.
    /// </summary>
    public T CreateUIPanel<T>() where T : UIPanelBase
    {
        string panelName = typeof(T).Name;
        T panel = FactoryPanel<T>();
        panel.Show();
        return panel as T;
    }

    /// <summary>
    /// 独立创建UI
    /// 销毁需调用此类中的Destroy_CreateUIPanel函数.
    /// </summary>
    public UIPanelBase CreateUIPanel(string panelName)
    {
        UIPanelBase panel = FactoryPanel(panelName);
        panel.Show();
        return panel;
    }

    /// <summary>
    /// 销毁独立创建的UI.
    /// </summary>
    public void Destroy_CreateUIPanel(UIPanelBase uiPanel)
    {
        if (uiPanel != null)
        {
            GameObject.Destroy(uiPanel.gameObject);
            uiPanel = null;
        }
    }

    /// <summary>
    /// 创建UI.
    /// </summary>
    private T FactoryPanel<T>() where T : UIPanelBase
    {
        return UIPanelBase.FactoryPanel<T>();
    }

    /// <summary>
    /// 创建UI.
    /// </summary>
    private UIPanelBase FactoryPanel(string panelName)
    {
        return UIPanelBase.FactoryPanel(panelName);
    }

    #region Expand

    public void Render()
    {
        uiCam.Render();
    }

    /// <summary>
    /// 世界坐标转UI坐标
    /// </summary>
    public Vector3 WorldPosToUIPos(Vector3 worldPos)
    {
        worldPos = mainCam.WorldToViewportPoint(worldPos);
        worldPos = uiCam.ViewportToWorldPoint(worldPos);
        return worldPos;
    }

    /// <summary>
    /// UI坐标转世界坐标
    /// </summary>
    public Vector3 UIPosToWorldPos(Vector3 uiPos)
    {
        uiPos = uiCam.WorldToViewportPoint(uiPos);
        uiPos = mainCam.ViewportToWorldPoint(uiPos);
        return uiPos;
    }

    /*
     开始：刚从地球上发射到太空的宇宙飞船内
     A:我们可能是人类最后一批宇航员了，哎。
     B:Why?
     A:你看看窗户外面。
     B:我的天哪？这是宇宙？你确定不是垃圾场？
     A:他们就是派我们来当清洁工的，不然你觉得就凭我们两个这个样子为什么能当时宇航员？
     B:……
     嗡……一个飞速运动残破的机械臂和飞船擦身而过
     A:该死！差点就被撞到了，我真的不该要这份钱。
     二人到达空间站。
     联合士官:系统预测一个小时后会有太空垃圾群会飞向空间站，千万别让这些垃圾撞上，为了拯救人类最后的空间站，我们给你们配备了最先进的设备，用尽全力使用亚空间吸收器收集垃圾。要是任务失败，你们清楚后果。
     */

    public void PlayTalk_Level1(Action onComplete)
    {
        var uiPanel = OpenUIPanel<UIPanelScreenTalk>();
        TalkData[] talks = new[]
        {
            new TalkData("A员", "我们可能是人类最后一批宇航员了，哎。", "烦", "1.我们"),
            new TalkData("B员", "Why?", null, "2.Wh"),
            new TalkData("A员", "你看看窗户外面。", null, "3.你看"),
            new TalkData("B员", "我的天哪？这是宇宙？你确定不是垃圾场？", null, "4.我的"),
            new TalkData("A员", "他们就是派我们来当清洁工的，不然你觉得就凭我们两个这个样子为什么能当时宇航员？", null, "5.他们"),
            new TalkData("B员", "……", "汗", null),
            //嗡……一个飞速运动残破的机械臂和飞船擦身而过
            new TalkData("A员", "该死！差点就被撞到了，我真的不该要这份钱。", "烦", "6.该死"),
            //二人到达空间站。
            new TalkData("联合士官",
                "系统预测一个小时后会有太空垃圾群会飞向空间站，千万别让这些垃圾撞上。为了拯救人类最后的空间站，我们给你们配备了最先进的设备，用尽全力使用亚空间吸收器收集垃圾。要是任务失败，你们清楚后果。",
                null, "7.系统"),
        };
        uiPanel.Show(talks, onComplete);
    }

    public void PlayTalk_Level2(Action onComplete)
    {
        /*
         关卡2：
         联合士官：太空码头储备的能源和物资，是人类未来飞出太阳系的关键，既然你们完成了第一天的任务，相信你们对“拾荒”已经得心应手了
         AB：拾荒……
         联合士官：咳，出发吧！
        */
        var uiPanel = OpenUIPanel<UIPanelScreenTalk>();
        TalkData[] talks = new[]
        {
            new TalkData("联合士官", "太空码头储备的能源和物资，是人类未来飞出太阳系的关键，既然你们完成了第一天的任务，相信你们对“拾荒”已经得心应手了", null, "8.你们"),
            new TalkData("A员", "拾……", "汗", "2.Wh"),
            new TalkData("B员", "拾荒……", "汗", "3.你看"),
            new TalkData("联合士官", "咳，出发吧！", null, "7.系统"),
        };
        uiPanel.Show(talks, onComplete);
    }

    public void PlayTalk_Level3(Action onComplete)
    {
        /*
         关卡3：
         联合士官：告诉你们一个很不幸的消息，有两个大型太空垃圾群分别飘向了你们祖国最重要的网络卫星，时间紧迫，如何选择营救方案是你们的自由。
         争执：
         A：你先和我去α卫星，等垃圾群飘过去马上就去β卫星。
         B：先去了α卫星，β卫星必然会被撞毁，我要去β！
         A：我是船长，我级别比你高！服从命令！
         B：你少拿这种虚衔压我，士官已经允许我们自由选择，我不跟你废话
         B说着就飘向β，A也不示弱去往α，他们似乎忘记他们之间有根牵引绳。
        */
        var uiPanel = OpenUIPanel<UIPanelScreenTalk>();
        TalkData[] talks = new[]
        {
            new TalkData("联合士官", "告诉你们一个很不幸的消息。有两个大型太空垃圾群分别飘向了你们祖国最重要的网络卫星，时间紧迫，如何选择营救方案是你们的自由。", null, "9.不幸"),
            new TalkData("A员", "你先和我去α卫星，等垃圾群飘过去马上就去β卫星。", null, "10.你先"),
            new TalkData("B员", "先去了α卫星，β卫星必然会被撞毁，我要去β！", null, "11.先去"),
            new TalkData("A员", "我是船长，我级别比你高！服从命令！", "烦", "12.我是"),
            new TalkData("B员", "你少拿这种虚衔压我，士官已经允许我们自由选择，我不跟你废话", "烦", "13.你少"),
            //B说着就飘向β，A也不示弱去往α，他们似乎忘记他们之间有根牵引绳。
        };
        uiPanel.Show(talks, onComplete);
    }

    #endregion
}