using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class SpaceStation : MonoBehaviour
{
    public int max_time = 5;
    int cur_time = 5;
    public Text text;

    private void Awake()
    {
        cur_time = max_time;
        text.text = cur_time + "/" + max_time;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Garbage"))
        {
            Destroy(collision.gameObject);
            cur_time--;
            if (cur_time == 0)
            {
                Time.timeScale = 0;
                var resultPanel = UIMgr.Inst.OpenUIPanel<UIPanelBattleResult>();
                resultPanel.ShowFail(() =>
                {
                    var uiPanel = UIMgr.Inst.OpenUIPanel<UIPanelBlackLerp>();
                    uiPanel.Fadein(() => { GameMain.Instance.ReloadCurScene(); });
                });
            }
            else
            {
                transform.DOShakePosition(0.4f, .1f).SetLoops(1, LoopType.Yoyo);
                transform.DOShakeScale(.2f, .1f).SetLoops(1, LoopType.Yoyo);
                GetComponent<SpriteRenderer>().color += new Color(0.2f, 0, 0);
                GetComponentInChildren<SpriteRenderer>().color += new Color(0.2f, 0, 0);
                text.text = cur_time + "/" + max_time;
            }
        }
    }
}