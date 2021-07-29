using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelBlackLerp : UIPanelBase
{
    public Animator _Animator;

    private Action onComplete;

    public void Fadein(Action onComplete)
    {
        this.onComplete = onComplete;
        transform.SetAsLastSibling();
        _Animator.Play("Show", 0, 0);
    }

    public void Fadeout(Action onComplete)
    {
        this.onComplete = onComplete;
        transform.SetAsLastSibling();
        _Animator.Play("Hide", 0, 0);
    }

    public void AnimEvent_OnComplete()
    {
        UIMgr.Inst.HideUIPanel<UIPanelBattleResult>();
        if (onComplete != null)
        {
            var act = onComplete;
            onComplete = null;
            act();
        }
    }
}