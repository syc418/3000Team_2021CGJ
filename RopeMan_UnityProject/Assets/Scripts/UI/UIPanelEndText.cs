using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelEndText : UIPanelBase
{
    public Animator _Animator;

    private Action onComplete;

    public void Show(Action onComplete)
    {
        this.onComplete = onComplete;
        _Animator.Play("Show", 0, 0);
    }

    public void AnimEvent_OnComplete()
    {
        if (onComplete != null)
        {
            var act = onComplete;
            onComplete = null;
            act();
        }
    }
}