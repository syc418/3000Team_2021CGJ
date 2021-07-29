using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMgr
{
    public static SoundMgr Inst { get; } = new SoundMgr();

    private AudioSource sourceCmp;

    public SoundMgr()
    {
        GameObject camObj = GameMain.Instance.gameObject;
        sourceCmp = camObj.GetComponent<AudioSource>();
        if (sourceCmp == null)
            sourceCmp = camObj.AddComponent<AudioSource>();
    }

    public void PlayOnce(string fileName, float volume = 1.0f)
    {
        if (string.IsNullOrEmpty(fileName))
            return;
        var clip = Resources.Load<AudioClip>($"Sounds/{fileName}");
        sourceCmp.PlayOneShot(clip, volume);
    }
}