using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelMainMenu : UIPanelBase
{
    public Animator _Animator;
    public Button _StartBtn;
    public Button _CloseBtn;

    protected override void Initialize()
    {
        _StartBtn.onClick.AddListener(OnStartGameBtnClick);
        _CloseBtn.onClick.AddListener(OnCloseControlDescBtnClick);
        base.Initialize();
    }

    public override void Show()
    {
        base.Show();
        _Animator.Play("Show", 0, 0);
    }

    private void OnStartGameBtnClick()
    {
        _Animator.Play("ControlDesc", 0, 0);
    }

    private void OnCloseControlDescBtnClick()
    {
        _Animator.Play("Hide", 0, 0);
    }

    public void AnimEvent_StartGame()
    {
        var uiPanel = UIMgr.Inst.OpenUIPanel<UIPanelBlackLerp>();
        uiPanel.Fadein(GameMain.Instance.GameStart);
    }
}