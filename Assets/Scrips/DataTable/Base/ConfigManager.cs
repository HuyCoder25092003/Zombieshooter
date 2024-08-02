using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigManager : BYSingletonMono<ConfigManager>
{
    public ConfigMission configMission;
    public ConfigShop configShop;
    public ConfigWeapon configWeapon;
    public ConfigEnemy configEnemy;
    public ConfigWaves configWaves;
    public void InitConfig(Action callback)
    {
        StartCoroutine(ProgressLoadConfig(callback));
    }
    IEnumerator ProgressLoadConfig(Action callback)
    {
        configShop = Resources.Load("Config/ConfigShop", typeof(ScriptableObject)) as ConfigShop;
        yield return new WaitUntil(() => configShop != null);

        configMission = Resources.Load("Config/ConfigMission", typeof(ScriptableObject)) as ConfigMission;
        yield return new WaitUntil(() => configShop != null);

        configWeapon = Resources.Load("Config/ConfigWeapon", typeof(ScriptableObject)) as ConfigWeapon;
        yield return new WaitUntil(() => configWeapon != null);
        
        configEnemy = Resources.Load("Config/ConfigEnemy", typeof(ScriptableObject)) as ConfigEnemy;
        yield return new WaitUntil(() => configEnemy != null);

        configWaves = Resources.Load("Config/ConfigWaves", typeof(ScriptableObject)) as ConfigWaves;
        yield return new WaitUntil(() => configWaves != null);

        callback?.Invoke();
    }
}
