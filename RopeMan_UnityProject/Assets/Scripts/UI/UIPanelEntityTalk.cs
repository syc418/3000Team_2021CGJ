using System;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelEntityTalk : UIPanelBase
{
    public Animator _Animator;
    public Transform _Parent;
    public Transform _DetectPoint;
    public Image _BgImg;
    public Text _Text;
    public Text _Text_Scale;
    public Vector2 _BgScaleOffset;

    public Transform _SirIcon;
    public Transform _SirIconPos;

    private bool isClick;
    private int dir;
    private Action onComplete;

    private static readonly int hashShowTag = Animator.StringToHash("Show");
    private static readonly int hashHideTag = Animator.StringToHash("Hide");

    /// <summary>
    /// 设置样式
    /// </summary>
    public void GraphicStyle(int dir, Color bgColor, Color textColor)
    {
        this.dir = dir;
        _BgImg.color = bgColor;
        _Text.color = textColor;

        Vector3 localScale = new Vector3(dir, 1, 1);
        _BgImg.rectTransform.localScale = localScale;
        _Text.rectTransform.localScale = localScale;
        _Text_Scale.rectTransform.localScale = localScale;
        _SirIcon.gameObject.SetActive(false);
    }

    /// <summary>
    /// 显示长官文字
    /// </summary>
    public void ShowText_Sir(string text, Action onComplete)
    {
        GraphicStyle(1, Color.blue, Color.white);
        _SirIcon.gameObject.SetActive(true);
        ShowText(Vector2.zero, text, onComplete);
        _Parent.position = _SirIconPos.position;
    }

    /// <summary>
    /// 显示文字，鼠标点击之后调用onComplete
    /// </summary>
    public void ShowText(Vector2 pos, string text, Action onComplete)
    {
        _Parent.position = UIMgr.Inst.WorldPosToUIPos(pos);

        _Text.text = text;
        _Text_Scale.text = text;
        UIMgr.Inst.Render();

        // float offsetH = dir > 0
        //     ? Mathf.Max(_DetectPoint.position.x - UIMgr.Inst.UIPosMax.x, 0) * -1
        //     : Mathf.Max(UIMgr.Inst.UIPosMin.x - _DetectPoint.position.x, 0);
        // _Parent.position += Vector3.right * offsetH;

        _Text.rectTransform.sizeDelta = _Text_Scale.rectTransform.sizeDelta;
        _BgImg.rectTransform.sizeDelta = _Text_Scale.rectTransform.sizeDelta + _BgScaleOffset;

        this.onComplete = onComplete;
        isClick = false;
        _Animator.Play(hashShowTag, 0, 0);
    }

    private void Update()
    {
        if (!isClick)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _Animator.Play(hashHideTag, 0, 0);
                isClick = true;
            }
        }
    }

    public void AnimEvent_PlayComplete()
    {
        if (onComplete != null)
        {
            var act = onComplete;
            onComplete = null;
            act();
        }
    }
}