using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelScreenTalk : UIPanelBase
{
    public Animator _Animator;
    public Sprite _TalkBgSprite_L;
    public Sprite _TalkBgSprite_R;
    public Sprite _Sprite_RoleA;
    public Sprite _Sprite_RoleB;
    public Sprite _Sprite_RoleC;
    public Vector2 _NameLocalPos_L;
    public Vector2 _NameLocalPos_R;
    public Sprite _MoodSprite_Happy;
    public Sprite _MoodSprite_Trouble;
    public Sprite _MoodSprite_Sweat;
    public Vector2 _MoodSpritePos_L;
    public Vector2 _MoodSpritePos_R;

    public Image _TalkBg;
    public Text _TalkText;
    public Text _TalkNameText;
    public Image _TalkSprite_L;
    public Image _TalkSprite_R;
    public Image _MoodSprite;

    private bool isPlaying;
    private bool isClick;
    private Queue<TalkData> curTalkQueue;
    private Action onComplete;

    protected override void Initialize()
    {
        curTalkQueue = new Queue<TalkData>();
        base.Initialize();
    }

    private void Update()
    {
        if (isPlaying && !isClick)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isClick = true;
                NextTalk();
            }
        }
    }

    /// <summary>
    /// 显示对话
    /// </summary>
    public void Show(TalkData[] talkDatas, Action onComplete)
    {
        UIMgr.Inst.HideUIPanel<UIPanelBattleResult>();
        curTalkQueue.Clear();
        for (int i = 0; i < talkDatas.Length; i++)
            curTalkQueue.Enqueue(talkDatas[i]);

        if (curTalkQueue.Count == 0)
        {
            onComplete?.Invoke();
            return;
        }

        isPlaying = true;
        this.onComplete = onComplete;
        NextTalk();
        _Animator.Play("Show", 0, 0);
    }

    public override void Hide()
    {
        _Animator.Play("Hide", 0, 0);
    }

    private void AnimEvent_Hide()
    {
        base.Hide();
    }

    private void NextTalk()
    {
        if (curTalkQueue.Count > 0)
        {
            isClick = false;
            var curTalkData = curTalkQueue.Dequeue();
            _TalkNameText.text = curTalkData.Name;
            _TalkText.text = curTalkData.Text;

            var sprite = GetMoodSpriteBySign(curTalkData.MoodSign);
            _MoodSprite.sprite = sprite;
            _MoodSprite.color = ReferenceEquals(sprite, null) ? new Color(0, 0, 0, 0) : Color.white;

            var componentData = GetTalkComponentDataByName(curTalkData.Name);
            _TalkBg.sprite = componentData.TalkBgSprite;
            if (componentData.RoleSprite_L != null)
            {
                //if (_TalkSprite_L.sprite != componentData.RoleSprite_L)
                _Animator.SetTrigger("RoleIconUpdate_L");
                _TalkSprite_L.sprite = componentData.RoleSprite_L;
            }

            if (componentData.RoleSprite_R != null)
            {
                //if (_TalkSprite_R.sprite != componentData.RoleSprite_R)
                _Animator.SetTrigger("RoleIconUpdate_R");
                _TalkSprite_R.sprite = componentData.RoleSprite_R;
            }

            _TalkNameText.rectTransform.anchoredPosition = componentData.NameLocalPosition;
            _TalkNameText.color = componentData.TalkTextColor;
            _TalkText.color = componentData.TalkTextColor;

            if (componentData.PosDirSign > 0)
            {
                _MoodSprite.transform.localScale = Vector3.one;
                _MoodSprite.rectTransform.anchoredPosition = _MoodSpritePos_R;
            }
            else
            {
                _MoodSprite.transform.localScale = new Vector3(-1, 1, 1);
                _MoodSprite.rectTransform.anchoredPosition = _MoodSpritePos_L;
            }

            SoundMgr.Inst.PlayOnce(curTalkData.SoundName);
        }
        else
        {
            Hide();
            isPlaying = false;
            if (onComplete != null)
            {
                var act = onComplete;
                onComplete = null;
                act();
            }
            else
            {
                Hide();
            }
        }
    }

    private Sprite GetMoodSpriteBySign(string sign)
    {
        switch (sign)
        {
            case "开心": return _MoodSprite_Happy;
            case "烦": return _MoodSprite_Trouble;
            case "汗": return _MoodSprite_Sweat;
            default: return null;
        }
    }

    private TalkComponentData GetTalkComponentDataByName(string name)
    {
        switch (name)
        {
            case "A员":
                return new TalkComponentData
                {
                    TalkBgSprite = _TalkBgSprite_L,
                    RoleSprite_L = _Sprite_RoleA,
                    NameLocalPosition = _NameLocalPos_L,
                    TalkTextColor = Color.black,
                    PosDirSign = -1,
                };
            case "B员":
                return new TalkComponentData
                {
                    TalkBgSprite = _TalkBgSprite_R,
                    RoleSprite_R = _Sprite_RoleB,
                    NameLocalPosition = _NameLocalPos_R,
                    TalkTextColor = Color.white,
                    PosDirSign = 1,
                };
            case "联合士官":
                return new TalkComponentData
                {
                    TalkBgSprite = _TalkBgSprite_L,
                    RoleSprite_L = _Sprite_RoleC,
                    NameLocalPosition = _NameLocalPos_L,
                    TalkTextColor = Color.black,
                    PosDirSign = -1,
                };
            default: throw new Exception("error: no cantains available role name");
        }
    }

    private struct TalkComponentData
    {
        public Sprite TalkBgSprite;
        public Sprite RoleSprite_L;
        public Sprite RoleSprite_R;
        public Vector2 NameLocalPosition;
        public Color TalkTextColor;
        public int PosDirSign;
    }
}

public struct TalkData
{
    public string Name;
    public string Text;
    public string MoodSign;
    public string SoundName;

    public TalkData(string name, string text, string moodSign, string soundName)
    {
        Name = name;
        Text = text;
        MoodSign = moodSign;
        SoundName = soundName;
    }
}

/*
5对话
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

关卡2：
联合士官：太空码头储备的能源和物资，是人类未来飞出太阳系的关键，既然你们完成了第一天的任务，相信你们对“拾荒”已经得心应手了
AB：拾荒……
联合士官：咳，出发吧！

关卡3：
联合士官：告诉你们一个很不幸的消息，有两个大型太空垃圾群分别飘向了你们祖国最重要的网络卫星，时间紧迫，如何选择营救方案是你们的自由。

争执：
A：你先和我去α卫星，等垃圾群飘过去马上就去β卫星。
B：先去了α卫星，β卫星必然会被撞毁，我要去β！
A：我是船长，我级别比你高！服从命令！
B：你少拿这种虚衔压我，士官已经允许我们自由选择，我不跟你废话
B说着就飘向β，A也不示弱去往α，他们似乎忘记他们之间有根牵引绳。
 */