using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    private float curDuration;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            // var uiPanel = UIMgr.Inst.OpenUIPanel<UIPanelEntityTalk>();
            // uiPanel.GraphicStyle(1, Color.white, Color.black);
            // uiPanel.ShowText(Vector2.zero, "你先和我去α卫星，等垃圾群飘过去马上就去β卫星。", OnTestTalkComplete);

            // var uiPanel = UIMgr.Inst.OpenUIPanel<UIPanelScreenTalk>();
            // uiPanel.Show(new[]
            // {
            //     new TalkData("A员", "我要踢你们的屁股！！！！", "烦", null),
            //     new TalkData("B员", "我看你是在想peach！！！！", null, null),
            //     new TalkData("A员", "我没有！！！！！！！", "汗", null),
            //     new TalkData("B员", "你太无聊了！！！！！！！", null, null),
            //     new TalkData("A员", "被发现了！！！", "开心", null),
            //     new TalkData("B员", "芜湖！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！", "开心", null),
            // }, () => { UIMgr.Inst.HideUIPanel<UIPanelScreenTalk>(); });
            UIMgr.Inst.PlayTalk_Level1(null);
        }

        var battlePanel = UIMgr.Inst.OpenUIPanel<UIPanelBattleStatus>();
        //battlePanel.OnPlayerAMassUpdated(0, Mathf.Abs(Mathf.Sin(Time.time * 0.5f)));
        //battlePanel.OnPlayerBMassUpdated(0, Mathf.Abs(Mathf.Cos(Time.time * 0.5f)));
        battlePanel.OnPlayerACompletionUpdated(Vector2.left, Mathf.Abs(Mathf.Cos(Time.time * 0.5f)));
        battlePanel.OnPlayerBCompletionUpdated(Vector2.right, Mathf.Abs(Mathf.Cos(Time.time * 0.5f)));

        if ((curDuration -= Time.deltaTime) <= 0)
        {
            battlePanel.OnPlayerAPowerUpdated((int) (Mathf.Abs(Mathf.Sin(Time.time * 0.25f)) * 10));
            battlePanel.OnPlayerBPowerUpdated((int) (Mathf.Abs(Mathf.Cos(Time.time * 0.25f)) * 10));
            battlePanel.OnShipHpUpdated(Mathf.Abs(Mathf.Sin(Time.time)));
            curDuration = 2.0f;
        }
    }

    // private void OnTestTalkComplete()
    // {
    //     var uiPanel = UIMgr.Inst.OpenUIPanel<UIPanelEntityTalk>();
    //     uiPanel.ShowText_Sir("我说完了！谁赞同谁反对！！！！！", OnTestTalkComplete);
    // }
}