using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class IngameUI : BYSingletonMono<IngameUI>
{
    public Image icon;
    public Text name_gun;
    public Text gun_Ammo_lb;
    public Image reload_pg;
    WeaponBehaviour cur_wp;
    public GameObject reload_ob;
    IEnumerator Start()
    {
        WeaponControl.instance.OnChangeGun.AddListener(OnChangeGun);
        yield return new WaitForSeconds(0.2f);
    }
    void OnChangeGun(WeaponBehaviour wp)
    {
        reload_ob.SetActive(false);
        StopCoroutine("ReloadProgress");
        if (cur_wp != null)
        {
            cur_wp.OnBulletChange.RemoveListener(OnBulletChange);
            cur_wp.OnReload.RemoveListener(Cur_wp_OnReload);
        }
        Debug.LogError("Wp: " + wp.gunDataIngame.cf.Prefab);
        cur_wp = wp;
        cur_wp.OnBulletChange.AddListener(OnBulletChange);
        cur_wp.OnReload.AddListener(Cur_wp_OnReload);
        gun_Ammo_lb.text = wp.number_bullet.ToString();
        icon.overrideSprite = SpriteLibControl.instance.GetSpriteByName(wp.gunDataIngame.cf.Prefab);
        name_gun.text = wp.gunDataIngame.cf.Name;
    }
    void OnBulletChange(int obj)
    {
        gun_Ammo_lb.text = obj.ToString();
    }
    void Cur_wp_OnReload(float obj)
    {
        reload_ob.SetActive(true);
        if (gameObject.activeInHierarchy)
        {
            StopCoroutine("ReloadProgress");
            StartCoroutine("ReloadProgress", obj);
        }
    }
    IEnumerator ReloadProgress(float reloadTime)
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);

        float count_time = 0;
        while (count_time < reloadTime)
        {
            yield return wait;
            count_time += 0.1f;
            reload_pg.fillAmount = count_time / reloadTime;

        }
        reload_ob.SetActive(false);
    }
}
