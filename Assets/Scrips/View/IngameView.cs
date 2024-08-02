using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameView : BaseView
{
    public Image fill_hp;
    public RectTransform parent_hub;
    public TMP_Text wave_lb;
    public override void Setup(ViewParam param)
    {
        base.Setup(param);
    }
    public override void OnShowView()
    {
        fill_hp.fillAmount = 1;
        MissionManager.instance.OnWaveChange.AddListener(OnWaveChange);
        MissionManager.instance.OnHPChange.AddListener(OnBaseHpChange);
    }
    void OnBaseHpChange(int hp, int max_hp)
    {
        float val = (float)hp / (float)max_hp;
        float x = val * 980f;

        fill_hp.rectTransform.sizeDelta = new Vector2(x, 50);
    }
    void OnWaveChange(int cur_wave, int max_wave)
    {
        wave_lb.text = cur_wave.ToString() + "/" + max_wave.ToString();
    }
}
