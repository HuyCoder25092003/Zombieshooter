using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public enum GunType
{
    Assault = 1,
    HandGun = 2,
    ShotGun = 3,
    MachineGun = 4,
    MiniGun = 5,
    Rocket =6,

}
public class GunDataIngame
{
    public ConfigWeaponRecord cf;
    public PositionFire positionFire;
}
public abstract class WeaponBehaviour : MonoBehaviour
{
    public ShellControl shellControl;
    public GunType gunType;
    public GunDataIngame gunDataIngame;
    public AnimatorOverrideController overrideController;
    public CharacterDataBinding characterDataBinding;
    public IWeaponHandle iWeaponHandle;
    public CharacterControl characterControl;

    public int clip_size = 5; // kích thước
    public int number_bullet;
    public float rof; // tốc độ bắn trên phút
    public float min_accuracy = 20; // độ chính xác tối thiểu
    public float max_accuracy = 70; // độ chính xác tối đa
    public float reload_time;
    public int damage;
    public float recovery_accuracy = 0.5f;
    public float accuracy;
    public float range;
    public ConfigWeaponRecord cf;
    public float drop_accuracy = 5; // tăng độ accuracy (accu tăng thì chính xác giảm)
    public bool isFire;
    public bool isReloading;
    public float fire_time;

    public UnityEvent<int> OnBulletChange;
    public UnityEvent<float> OnReload;

    bool checkSemiAuto;
    public bool semi_auto = true;

    public bool isReloadInput;

    public MuzzleFlash muzzleFlash;
    public string name_bullet_pool;
    public Transform projecties_pb;
    public float force = 100;

    public float mutiple_speed_anim;

    public AudioSource audioSource_;
    public AudioClip[] sfx_fires;
    public AudioClip sfx_ready;
    public AudioClip sfx_reload;
    public virtual void Setup(GunDataIngame data)
    {
        damage = data.cf.Damge;
        rof = data.cf.ROF;
        min_accuracy = data.cf.Accuracy_min;
        max_accuracy = data.cf.Accuracy_max;
        reload_time = data.cf.Reload;
        range = data.cf.Range;
        accuracy = min_accuracy;
        cf = data.cf;
        this.gunDataIngame = data;
        number_bullet = clip_size;
        characterDataBinding = gameObject.GetComponentInParent<CharacterDataBinding>();
        characterControl = gameObject.GetComponentInParent<CharacterControl>();
        accuracy = min_accuracy;
        BYPool pool = new BYPool(name_bullet_pool, clip_size, projecties_pb);
        BYPoolManager.instance.AddPool(pool);

        Init();
    }
    public abstract void Init();
    public void HideGun()
    {
        gameObject.SetActive(false);
    }
    public void ReadyGun()
    {
        accuracy = min_accuracy;
        gameObject.SetActive(true);
        audioSource_.PlayOneShot(sfx_ready);
        characterDataBinding.SetSpeedReload(mutiple_speed_anim);
        characterDataBinding.PlayShowGun();
        if (isReloading)
            iWeaponHandle.ReloadHandle();
    }
    public void OnFire(bool isFire_)
    {
        this.isFire = isFire_;
        checkSemiAuto = true;
        fire_time = 0;
    }
    void Update()
    {
        fire_time += Time.deltaTime;
        if (semi_auto)
        {
            if (isFire && !isReloading)
            {
                if (number_bullet > 0 && fire_time >= rof)
                {
                    number_bullet--;
                    fire_time = 0;
                    if(gunType != GunType.Rocket)
                        muzzleFlash.FireHandle();
                    iWeaponHandle.FireHandle();
                    OnBulletChange?.Invoke(number_bullet);
                }
            }
        }
        else
        {
            if (isFire && !isReloading)
            {
                if (number_bullet > 0 && fire_time >= rof && checkSemiAuto)
                {
                    checkSemiAuto = false;
                    number_bullet--;
                    fire_time = 0;
                    if (gunType != GunType.Rocket)
                        muzzleFlash.FireHandle();

                    iWeaponHandle.FireHandle();

                    OnBulletChange?.Invoke(number_bullet);

                }
            }
        }
        accuracy = Mathf.Lerp(accuracy, min_accuracy, Time.deltaTime * recovery_accuracy);
    }
    public void Reload(bool isReload)
    {
        isReloadInput = isReload;
        if (isReloadInput)
            iWeaponHandle.ReloadHandle();
    }
}
