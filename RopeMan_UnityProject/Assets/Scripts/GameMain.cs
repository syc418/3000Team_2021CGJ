using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMain : MonoSingleton<GameMain>
{
    public AudioSource xiu;
    public int cur_scene = 1;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        UIMgr.Inst.OpenUIPanel<UIPanelMainMenu>();
        //var battlePanel = UIMgr.Inst.OpenUIPanel<UIPanelBattleStatus>();
        //battlePanel.ShowTitle("TITTLESLDIJFLSDJFJ","sdfsdf");
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Z))
        // {
        //     UIMgr.Inst.OpenUIPanel<UIPanelBattleResult>().ShowWin(null);
        // }
        //
        // if (Input.GetKeyDown(KeyCode.X))
        // {
        //     UIMgr.Inst.OpenUIPanel<UIPanelBattleResult>().ShowFail(null);
        // }
        //
        // if (Input.GetKeyDown(KeyCode.C))
        // {
        //     UIMgr.Inst.OpenUIPanel<UIPanelBattleResult>().ShowComplete(null);
        // }

        // if (Input.GetKeyDown((KeyCode.T)))
        // {
        //     Time.timeScale = 10;
        // }
        // if (Input.GetKeyUp((KeyCode.T)))
        // {
        //     Time.timeScale = 1;
        // }
    }

    public void GameStart()
    {
        UIMgr.Inst.HideUIPanel<UIPanelMainMenu>();
        cur_scene = 1;
        SceneManager.LoadScene(cur_scene);
        Time.timeScale = 0;

        var uiPanel = UIMgr.Inst.GetUIPanel<UIPanelBlackLerp>();
        if (uiPanel != null)
        {
            uiPanel.Fadeout(() => { UIMgr.Inst.PlayTalk_Level1(OnOK); });
        }
    }

    public void OnOK()
    {
        Time.timeScale = 1;
    }

    public void LoadNextScene()
    {
        cur_scene++;
        SceneManager.LoadScene(cur_scene);
    }

    public void ReloadCurScene()
    {
        Time.timeScale = 0;
        SceneManager.LoadScene(cur_scene);

        var uiPanel = UIMgr.Inst.GetUIPanel<UIPanelBlackLerp>();
        if (uiPanel != null)
        {
            uiPanel.Fadeout(OnOK);
        }
    }
}