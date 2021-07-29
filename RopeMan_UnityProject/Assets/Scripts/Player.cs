using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

public enum PlayerType
{
    A,
    B
}

public class Player : MonoBehaviour
{
    Rigidbody2D r2;
    public KeyCode upKey, downKey, leftKey, rightKey;
    public PlayerType p_type;
    public float attack_CD = 3;
    public float attack_range = 3;
    public float attack_delay = 1;
    private float attack_timer = 3;
    private float temp_attack_timer = 3;
    public SpriteRenderer atk_light;
    private bool checking = false;
    public GameObject sr;
    public Tween t;
    private int point = 0;
    public Transform leftHand;
    private Light2D l2d;
    public Animator fire;

    public Text text;
    public Image gunred;
    public Image gunno;
    public Image gunenergypro;
    public EnergyProgress energyProgress;

    //private UIPanelBattleStatus battle_ui;
    private float complate;

    // Start is called before the first frame update
    void Start()
    {
        r2 = GetComponent<Rigidbody2D>();
        atk_light.color = new Color(1, 1, 1, 0);
        atk_light.GetComponent<Light>().SetPlayer(this);
        l2d = atk_light.GetComponent<Light2D>();
        fire.gameObject.SetActive(false);
        text.text = "0/20";
        //talk.
        //battle_ui = UIMgr.Inst.GetUIPanel<UIPanelBattleStatus>();
        //switch (p_type)
        //{
        //    case PlayerType.A:
        //        battle_ui.OnPlayerAPowerUpdated(point);
        //        battle_ui.OnPlayerACompletionUpdated(transform.position, 0);
        //        break;
        //    case PlayerType.B:
        //        battle_ui.OnPlayerBPowerUpdated(point);
        //        battle_ui.OnPlayerBCompletionUpdated(transform.position, 0);
        //        break;
        //    default:
        //        break;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.LogError("curPoint: " + point);
        //switch (p_type)
        //{
        //    case PlayerType.A:
        //        battle_ui.OnPlayerACompletionUpdated(transform.position, complate);
        //        break;
        //    case PlayerType.B:
        //        battle_ui.OnPlayerBCompletionUpdated(transform.position, complate);
        //        break;
        //    default:
        //        break;
        //}
        //if(checking) CheckMonster();
        l2d.intensity = atk_light.color.a * 3;
        attack_timer -= Time.deltaTime;
        if (attack_timer <= 0)
        {
            temp_attack_timer =  attack_CD + Random.Range(-attack_range / 2, attack_range / 2);
            attack_timer = temp_attack_timer;
            checking = true;

            atk_light.DOFade(1, .5f).OnComplete(() =>
            {
                atk_light.DOFade(0, 0.5f).SetDelay(attack_delay).OnComplete(() =>
                {
                    checking = false;
                });
            });
        }

        float pro = 1;
        if (temp_attack_timer - attack_timer < 2)
        {
            pro = 1-(temp_attack_timer - attack_timer) / 2;
            gunenergypro.fillAmount = pro;
            
        }
        else if (temp_attack_timer - attack_timer >= 2)
        {
            // gunno.gameObject.SetActive(temp_attack_timer - attack_timer <= 3);
            pro = 1 - attack_timer / (temp_attack_timer - 2);
            gunenergypro.fillAmount = pro;
        }
        energyProgress.SetEnergyProgress(pro);
        gunred.gameObject.SetActive(temp_attack_timer - attack_timer <= 3&&temp_attack_timer - attack_timer >0.5);
        

        Vector2 f = Vector2.zero;
        if (Input.GetKey(upKey))
        {
            f = new Vector2(f.x, 1);
        }

        if (Input.GetKey(downKey))
        {
            f = new Vector2(f.x, -1);
        }

        if (Input.GetKey(leftKey))
        {
            f = new Vector2(-1, f.y);
        }

        if (Input.GetKey(rightKey))
        {
            f = new Vector2(1, f.y);
        }

        r2.velocity = Vector3.Lerp(r2.velocity, r2.velocity = f * 6, Time.deltaTime * 2f);
        var b = f != Vector2.zero;

        t.Kill();
        if (b && !fire.gameObject.activeInHierarchy)
        {
            fire.gameObject.SetActive(true);
        }

        if (!b && fire.gameObject.activeInHierarchy)
        {
            fire.gameObject.SetActive(false);
        }

        if (b)
        {
            float angle = Vector3.SignedAngle(Vector3.up, f, Vector3.forward);
            Quaternion rotation00 = Quaternion.Euler(0, 0, angle);

            t = sr.transform.DORotateQuaternion(rotation00, 0.4f);
            return;
        }
        //else
        //{

        //}
    }

    public void GetGarbage(Garbage garbage)
    {
        if (!checking) return;
        GameObject.Destroy(garbage.gameObject, 1);
        garbage.is_alive = false;
        garbage.GetComponent<Collider2D>().enabled = false;
        garbage.transform.DOScale(0, .4f).SetEase(Ease.InBack);
        garbage.transform.DOMove(transform.position, .4f).SetDelay(.2f);
        point++;
        r2.mass++;
        text.text = point + "/20";
        SoundMgr.Inst.PlayOnce($"ABS_0{Random.Range(1, 4)}");
        //switch (p_type)
        //{
        //    case PlayerType.A:
        //        battle_ui.OnPlayerAPowerUpdated(point);
        //        break;
        //    case PlayerType.B:
        //        battle_ui.OnPlayerBPowerUpdated(point);
        //        break;
        //    default:
        //        break;
        //}
        if (point >= 20)
        {
            Time.timeScale = 0;
            DontDestroyOnLoad(gameObject);
            // SceneManager.LoadScene(1);
            // Time.timeScale = 0;
            if (GameMain.Instance.cur_scene == 1)
            {
                var resultPanel = UIMgr.Inst.OpenUIPanel<UIPanelBattleResult>();
                resultPanel.ShowWin(() =>
                {
                    UIMgr.Inst.PlayTalk_Level2(() =>
                    {
                        var uiPanel = UIMgr.Inst.OpenUIPanel<UIPanelBlackLerp>();
                        uiPanel.Fadein(() =>
                        {
                            GameMain.Instance.LoadNextScene();
                            uiPanel.Fadeout(() => { Time.timeScale = 1; });
                        });
                    });
                });
            }
            else if (GameMain.Instance.cur_scene == 2)
            {
                var resultPanel = UIMgr.Inst.OpenUIPanel<UIPanelBattleResult>();
                resultPanel.ShowWin(() =>
                {
                    UIMgr.Inst.PlayTalk_Level3(() =>
                    {
                        var uiPanel = UIMgr.Inst.OpenUIPanel<UIPanelBlackLerp>();
                        uiPanel.Fadein(() =>
                        {
                            GameMain.Instance.LoadNextScene();
                            uiPanel.Fadeout(() => { Time.timeScale = 1; });
                        });
                    });
                });
            }
            else if (GameMain.Instance.cur_scene == 3)
            {
                var resultPanel = UIMgr.Inst.OpenUIPanel<UIPanelBattleResult>();
                resultPanel.ShowComplete(() =>
                {
                    var uiPanel = UIMgr.Inst.OpenUIPanel<UIPanelEndText>();
                    uiPanel.Show(() =>
                    {
                        UIMgr.Inst.HideUIPanel<UIPanelEndText>();
                        SceneManager.LoadScene(0);
                        UIMgr.Inst.OpenUIPanel<UIPanelMainMenu>();
                        Time.timeScale = 1;
                    });
                });
            }
        }
        //var battlePanel = UIMgr.Inst.OpenUIPanel<UIPanelBattleStatus>();
        //battlePanel.ShowTitle("TITTLESLDIJFLSDJFJ","sdfsdf");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Garbage"))
        {
            //碰到垃圾
            r2.velocity = -((Vector2) collision.gameObject.transform.position - r2.position).normalized * 10;
            collision.GetComponent<Rigidbody2D>().velocity = -r2.velocity / 20;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            //撞墙
            r2.velocity = -r2.velocity;
        }
    }
}