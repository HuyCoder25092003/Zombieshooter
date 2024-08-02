using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RocketWeapon : WeaponBehaviour
{
    public override void Init()
    {
        iWeaponHandle = new IRocket();
        iWeaponHandle.Init(this);
    }
    public override void Setup(GunDataIngame data)
    {
        base.Setup(data);
        characterControl.speed_move = 2;
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
public class IRocket : IWeaponHandle
{
    RocketWeapon wp;
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
        wp.audioSource_.PlayOneShot(wp.sfx_fires.OrderBy(x => Guid.NewGuid()).FirstOrDefault());
        Transform bl = BYPoolManager.instance.dic_pool[wp.name_bullet_pool].Spawn();
        bl.position = wp.gunDataIngame.positionFire.GetPosFire(out Vector3 dir);

        Rigidbody rb = bl.GetComponent<Rigidbody>();
        rb.AddForce(wp.gunDataIngame.positionFire.pos_fire.forward * wp.force, ForceMode.VelocityChange);

        GrenadeControl grenade = bl.gameObject.GetComponent<GrenadeControl>();
        Grenadedata data = new Grenadedata() { damage = 2, radius = (int)wp.range };
        grenade.Setup(data);
    }

    public void Init(WeaponBehaviour wp)
    {
        this.wp = (RocketWeapon)wp;
    }

    public void ReloadHandle()
    {
        wp.StartReload();
    }
}
