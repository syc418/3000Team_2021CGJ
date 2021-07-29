using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelBattleStatus : UIPanelBase
{
    //飞船血条显示时长
    private const float SHIP_SHOWTIME = 5.0f;

    public Animator _Animator;
    public Image _ShipHpImg;
    public Image _Player_1_PowerImg;
    public GameObject _Player_1_PowerBtnObj;

    public Image _Player_2_PowerImg;

    public GameObject _Player_2_PowerBtnObj;
    // public Image _Player_1_MassEmptyImg;
    // public Image _Player_1_MassFullImg;
    // public Image _Player_1_MassArrowImg;
    // public Image _Player_2_MassEmptyImg;
    // public Image _Player_2_MassFullImg;
    // public Image _Player_2_MassArrowImg;
    // public float _MassArrowHeightMin;
    // public float _MassArrowHeightMax;
    // public AnimationCurve _MassArrowHorizontalOffsetCurve;

    public Transform _Player_1_CompletionParent;
    public Transform _Player_2_CompletionParent;
    public RectTransform _Player_1_CompletionArrow;
    public RectTransform _Player_2_CompletionArrow;
    public float _CompletionOffsetMin;
    public float _CompletionOffsetMax;
    public float _CompletionHeightMax;
    public float _CompletionAngleMax;
    public AnimationCurve _SampCurve;

    public Text _Title_1_Text;
    public Text _Title_2_Text;
    public Text _Time_Text;

    public Button _MusicBtn;
    public Button _MusicMuteBtn;
    public Button _BackMenuBtn;
    public Button _PauseMenuBtn;
    public GameObject _BackMenuDiagObj;
    public Button _BackMenuDiagCloseBtn;
    public Button _BackMenuDiagCancelBtn;
    public Button _BackMenuDiagOKBtn;

    private float curShipShowDuration;

    private int animLayer_ShipHp;
    private int animLayer_Title;

    protected override void Initialize()
    {
        animLayer_ShipHp = _Animator.GetLayerIndex("ShipHp");
        // animLayer_Title = _Animator.GetLayerIndex("Title");
        curShipShowDuration = SHIP_SHOWTIME;

        //todo tempArgs: true => load music enable...
        UpdateMusicEnableStatus(true);
        _MusicBtn.onClick.AddListener(() =>
        {
            //设置音量为关
            //todo
            UpdateMusicEnableStatus(false);
        });
        _MusicMuteBtn.onClick.AddListener(() =>
        {
            //设置音量为开
            //todo
            UpdateMusicEnableStatus(true);
        });
        _BackMenuBtn.onClick.AddListener(() => { _BackMenuDiagObj.SetActive(true); });
        _BackMenuDiagCloseBtn.onClick.AddListener(() => { _BackMenuDiagObj.SetActive(false); });
        _BackMenuDiagCancelBtn.onClick.AddListener(() => { _BackMenuDiagObj.SetActive(false); });
        _BackMenuDiagOKBtn.onClick.AddListener(() =>
        {
            //todo 结束关卡，关闭全部ui，回到主菜单
            UIMgr.Inst.HideUIPanel<UIPanelBattleStatus>();
        });
        base.Initialize();
    }

    public void UpdateMusicEnableStatus(bool musicEnable)
    {
        _MusicBtn.gameObject.SetActive(musicEnable);
        _MusicMuteBtn.gameObject.SetActive(!musicEnable);
    }

    private void Update()
    {
        if (curShipShowDuration < SHIP_SHOWTIME &&
            (curShipShowDuration += Time.deltaTime) >= SHIP_SHOWTIME)
        {
            _Animator.Play("ShipHp_Hide", animLayer_ShipHp, 0);
        }
    }

    /// <summary>
    /// 显示标题动画
    /// </summary>
    public void ShowTitle(string title1, string title2)
    {
        _Animator.Play("Title_Show", animLayer_Title, 0);
    }

    //飞船血量更新
    public void OnShipHpUpdated(float rate)
    {
        _ShipHpImg.fillAmount = rate;
        if (curShipShowDuration >= 0)
            _Animator.Play("ShipHp_Show", 0, 0);
        curShipShowDuration = 0;
    }

    //玩家A能量值更新
    public void OnPlayerAPowerUpdated(int power)
    {
        _Player_1_PowerBtnObj.SetActive(power >= 8);
        _Player_1_PowerImg.fillAmount = power / 8f;
        _Animator.SetTrigger("Player_1_Power_Update");
    }

    //玩家B能量值更新
    public void OnPlayerBPowerUpdated(int power)
    {
        _Player_2_PowerBtnObj.SetActive(power >= 8);
        _Player_2_PowerImg.fillAmount = power / 8f;
        _Animator.SetTrigger("Player_2_Power_Update");
    }

    //玩家A完成度值更新
    public void OnPlayerACompletionUpdated(Vector2 worldPos, float rate)
    {
        _Player_1_CompletionParent.position = UIMgr.Inst.WorldPosToUIPos(worldPos);
        _Player_1_CompletionArrow.anchoredPosition = new Vector2(
            Mathf.Lerp(_CompletionOffsetMin, _CompletionOffsetMax, _SampCurve.Evaluate(rate)),
            Mathf.Lerp(-_CompletionHeightMax, _CompletionHeightMax, rate));
        _Player_1_CompletionArrow.localEulerAngles = new Vector3(0, 0,
            Mathf.Lerp(_CompletionAngleMax, -_CompletionAngleMax, rate));
    }

    //玩家B完成度值更新
    public void OnPlayerBCompletionUpdated(Vector2 worldPos, float rate)
    {
        _Player_2_CompletionParent.position = UIMgr.Inst.WorldPosToUIPos(worldPos);
        _Player_2_CompletionArrow.anchoredPosition = new Vector2(
            Mathf.Lerp(_CompletionOffsetMin, _CompletionOffsetMax, _SampCurve.Evaluate(rate)),
            Mathf.Lerp(-_CompletionHeightMax, _CompletionHeightMax, rate));
        _Player_2_CompletionArrow.localEulerAngles = new Vector3(0, 0,
            Mathf.Lerp(_CompletionAngleMax, -_CompletionAngleMax, rate));
    }

    // //玩家A重量值更新
    // public void OnPlayerAMassUpdated(float massValue, float rate)
    // {
    //     // _Player_1_MassEmptyImg.material.SetFloat("_Lerp", rate);
    //     // _Player_1_MassFullImg.material.SetFloat("_Lerp", 1 - rate);
    //     _Player_1_MassEmptyImg.fillAmount = 1 - rate;
    //     _Player_1_MassFullImg.fillAmount = rate;
    //
    //     Vector2 localPosition = new Vector2(_MassArrowHorizontalOffsetCurve.Evaluate(rate),
    //         Mathf.Lerp(_MassArrowHeightMin, _MassArrowHeightMax, rate));
    //     _Player_1_MassArrowImg.rectTransform.anchoredPosition = localPosition;
    // }
    //
    // //玩家A重量值更新
    // public void OnPlayerBMassUpdated(float massValue, float rate)
    // {
    //     // _Player_2_MassEmptyImg.material.SetFloat("_Lerp", rate);
    //     // _Player_2_MassFullImg.material.SetFloat("_Lerp", 1 - rate);
    //     _Player_2_MassEmptyImg.fillAmount = 1 - rate;
    //     _Player_2_MassFullImg.fillAmount = rate;
    //
    //     Vector2 localPosition = new Vector2(_MassArrowHorizontalOffsetCurve.Evaluate(rate) * -1,
    //         Mathf.Lerp(_MassArrowHeightMin, _MassArrowHeightMax, rate));
    //     _Player_2_MassArrowImg.rectTransform.anchoredPosition = localPosition;
    // }
}