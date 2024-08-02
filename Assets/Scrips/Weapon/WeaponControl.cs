using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class WeaponControl : BYSingletonMono<WeaponControl>
{
    public CharacterControl characterControl;
    public List<WeaponBehaviour> weapons;
    public List<int> id_wps;
    public Transform anchor_wp;
    private int index_wp = -1;
    private WeaponBehaviour cur_wp;
    public WeaponBehaviour Cur_WP => cur_wp;
    public UnityEvent<WeaponBehaviour> OnChangeGun;
    public List<PositionFire> positionFires;
    bool isActiveGun;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < id_wps.Count; i++)
        {
            weapons.Add(null);
        }
        InputManager.instance.OnFire.AddListener(OnFire);
        InputManager.instance.OnChangeGun.AddListener(ChangeGun);
        InputManager.instance.OnReload.AddListener(OnReload);
        foreach (int e in id_wps)
        {
            ConfigWeaponRecord cf_wp = ConfigManager.instance.configWeapon.GetRecordBykeySearch(e);
            Addressables.LoadAssetAsync<GameObject>("Assets/Resources_moved/Weapons/"+ cf_wp.Prefab +".prefab").Completed +=
            (asyncOperationHandle) =>
            {
                if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    isActiveGun = true;
                    GameObject go = Instantiate(asyncOperationHandle.Result);
                    Debug.LogError(cf_wp.Prefab);
                    WeaponBehaviour wp_behaviour = go.GetComponent<WeaponBehaviour>();
                    GunDataIngame data = new GunDataIngame();
                    data.cf = cf_wp;
                    go.transform.SetParent(anchor_wp, false);
                    go.SetActive(false);
                    if (isActiveGun)
                        GameObject.Find("NewInputSystem").GetComponent<PlayerInput>().enabled = true;
                    PositionFire positionFire = positionFires.Where(x => x.gunType == wp_behaviour.gunType).FirstOrDefault();
                    positionFire.characterControl = characterControl;
                    data.positionFire = positionFire;
                    wp_behaviour.Setup(data);
                    weapons[e - 1] = wp_behaviour;
                }
                else
                    Debug.LogError("Failed to load the gun prefab.");
            };
        }
        yield return new WaitForSeconds(2);
        ChangeGun();
    }
    public void OnFire(bool isFire)
    {
        cur_wp.OnFire(isFire);
    }
    void OnReload(bool isReload)
    {
        cur_wp.Reload(isReload);
    }
    void ChangeGun()
    {
        index_wp++;
        if (index_wp >= weapons.Count)
        {
            index_wp = 0;
        }
        WeaponBehaviour wp = weapons[index_wp];

        cur_wp?.HideGun();

        cur_wp = wp;

        OnChangeGun?.Invoke(wp);

        cur_wp.ReadyGun();
    }
    void OnDestroy()
    {
        OnChangeGun.RemoveAllListeners();
    }
}
[Serializable]
public class PositionFire
{
    public GunType gunType;
    public Transform pos_fire;
    public Transform aim;
    public CharacterControl characterControl;
    public Vector3 GetPosFire(out Vector3 dir)
    {
        if (characterControl.cur_tran_enemy != null)
        {
            dir = characterControl.cur_tran_enemy.position - pos_fire.position;
            dir.Normalize();
        }
        else
        {
            dir = aim.position - pos_fire.position;
            dir.Normalize();
        }

        return pos_fire.position;
    }
}
