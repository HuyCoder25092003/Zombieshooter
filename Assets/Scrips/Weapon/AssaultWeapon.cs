using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class AssaultWeapon : WeaponBehaviour
{
    public override void Init()
    {
        iWeaponHandle = new IAssault();
        iWeaponHandle.Init(this);
    }
    public override void Setup(GunDataIngame data)
    {
        base.Setup(data);
    }
    public void StartReload()
    {
        StopCoroutine("ReloadProgress");
        StartCoroutine("ReloadProgress");
    }
    IEnumerator ReloadProgress()
    {
        audioSource_.PlayOneShot(sfx_reload);

        isReloading = true;
        OnReload?.Invoke(reload_time);
        characterDataBinding.PlayReloadGun();
        yield return new WaitForSeconds(reload_time);
        isReloading = false;
        number_bullet = clip_size;
        OnBulletChange?.Invoke(number_bullet);
    }
}
public class IAssault : IWeaponHandle
{
    AssaultWeapon wp;

    public void FireHandle()
    {
        wp.characterDataBinding.PlayFireGun();
        CreateBullet();
        if (wp.number_bullet <= 0)
        {
            ReloadHandle();
        }
    }
    void CreateBullet()
    {
        wp.accuracy += wp.drop_accuracy;
        wp.accuracy = Mathf.Clamp(wp.accuracy, wp.min_accuracy, wp.max_accuracy);

        wp.shellControl.Fire();
        wp.audioSource_.PlayOneShot(wp.sfx_fires.OrderBy(x => Guid.NewGuid()).FirstOrDefault());

        Transform bl = BYPoolManager.instance.dic_pool[wp.name_bullet_pool].Spawn();
        bl.position = wp.gunDataIngame.positionFire.GetPosFire(out Vector3 dir);
        float accuracy_val = wp.accuracy * 0.08f;
        float x = UnityEngine.Random.Range(-accuracy_val, accuracy_val);
        float y = UnityEngine.Random.Range(-accuracy_val, accuracy_val);
        Quaternion q = Quaternion.Euler(x, y, 0);
        bl.forward = q * dir;

        BulletControl bl_control = bl.GetComponent<BulletControl>();
        Bulletdata bulletdata = new Bulletdata { damage = wp.damage, name_pool = wp.name_bullet_pool };
        bulletdata.force = wp.force * bl.forward;
        bl_control.Setup(bulletdata);
    }
    public void Init(WeaponBehaviour wp)
    {
        this.wp = (AssaultWeapon)wp;
    }

    public void ReloadHandle()
    {
        wp.StartReload();
    }
}
