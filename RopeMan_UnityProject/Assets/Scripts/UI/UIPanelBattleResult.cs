using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelBattleResult : UIPanelBase
{
    public Animator _Animator;
    public Button _Btn;

    private Action onComplete;

    protected override void Initialize()
    {
        _Btn.onClick.AddListener(OnButtonClick);
        base.Initialize();
    }

    public void ShowWin(Action onComplete)
    {
        this.onComplete = onComplete;
        _Animator.Play("Win", 0, 0);
    }

    public void ShowFail(Action onComplete)
    {
        this.onComplete = onComplete;
        _Animator.Play("Fail", 0, 0);
    }

    public void ShowComplete(Action onComplete)
    {
        this.onComplete = onComplete;
        _Animator.Play("Complete", 0, 0);
    }

    public void OnButtonClick()
    {
        if (onComplete != null)
        {
            var act = onComplete;
            onComplete = null;
            act();
        }
    }
}