using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Linq;

public class MissionManager : BYSingletonMono<MissionManager>
{
    public ConfigMissionRecord cf_mission;
    List<int> waves;
    public int hp;
    private int max_hp;
    public UnityEvent<int, int> OnHPChange;
    public UnityEvent<int, int> OnWaveChange;
    [SerializeField] int index_wave = -1;
    [SerializeField] int total_enemy;
    int count_enemy_create;
    [SerializeField] int number_enemy_dead;
    public UnityEvent OnDied;
    bool isEndMission = false;
    IEnumerator Start()
    {
        cf_mission = GameManager.instance.cur_cf_mission;
        waves = cf_mission.Waves;
        yield return new WaitForSeconds(4);
        StartCoroutine("CreateNewWaves");
        StartCoroutine("CreatePlayer");
    }
    IEnumerator CreateNewWaves()
    {
        index_wave++;
        number_enemy_dead = 0;
        if (index_wave >= waves.Count)
        {
            OnWaveChange.RemoveAllListeners();
            OnHPChange.RemoveAllListeners();
            Debug.LogError(" mission complete ");
            IngameUI.instance.gameObject.SetActive(false);
            BYPoolManager.instance.GetPool("HpHub").DeSpawnAll();
            WinDialogParam param = new WinDialogParam();
            param.cf_mission = cf_mission;
            DialogManager.instance.ShowDialog(DialogIndex.WinDialog, param);
        }
        else
        {
            ConfigWaveRecord cf_wave = ConfigManager.instance.configWaves.GetRecordBykeySearch(waves[index_wave]);
            total_enemy = cf_wave.Enemies.Count;
            count_enemy_create = number_enemy_dead = 0;
            OnWaveChange?.Invoke(index_wave + 1, waves.Count);
            yield return new WaitForSeconds(cf_wave.Delay);
            List<int> id_enemies = cf_wave.Enemies;
            for (int i = 0; i < total_enemy; i++)
                StartCoroutine(CreateEnemy(cf_wave.Delay, id_enemies.OrderBy(x => Guid.NewGuid()).FirstOrDefault()));
        }
    }
    IEnumerator CreateEnemy(int delay, int id)
    {
        yield return new WaitForSeconds(delay);

        //Create Enemy
        count_enemy_create++;
        ConfigEnemyRecord cf_enemy = ConfigManager.instance.configEnemy.GetRecordBykeySearch(id);
        Addressables.LoadAssetAsync<GameObject>("Assets/Resources_moved/Enemies/" + cf_enemy.Prefab + ".prefab").Completed +=
            (asyncOperationHandle) =>
            {
                if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject e_obj = Instantiate(asyncOperationHandle.Result);
                    EnemyControl enemyControl = e_obj.GetComponent<EnemyControl>();
                    enemyControl.Setup(new EnemyInitData { cf = cf_enemy });
                }
                else
                {
                    Debug.LogError("Failed to load the enemy prefab.");
                }
            };
    }
    public void OnDamage(DamageData damageData)
    {
        hp -= damageData.damage;
        if (hp <= 0)
        {
            hp = 0;
            OnHPChange?.Invoke(hp, max_hp);
            if (!isEndMission)
            {
                OnWaveChange.RemoveAllListeners();
                OnHPChange.RemoveAllListeners();
                IngameUI.instance.gameObject.SetActive(false);
                BYPoolManager.instance.GetPool("HpHub").DeSpawnAll();
                Debug.LogError("LoseGame");
                DialogManager.instance.ShowDialog(DialogIndex.FailDialog);
                OnDied?.Invoke();
                isEndMission = true;
            }
        }
        else
            OnHPChange?.Invoke(hp, max_hp);
    }
    public void EnemyDead(EnemyControl e)
    {
        number_enemy_dead++;
        if (count_enemy_create >= total_enemy && number_enemy_dead >= total_enemy)
            StartCoroutine("CreateNewWaves");
    }
    IEnumerator CreatePlayer()
    {
        yield return new WaitForSeconds(1);
        Addressables.LoadAssetAsync<GameObject>("Assets/Resources_moved/Player/Logic.prefab").Completed +=
            (asyncOperationHandle) =>
            {
                if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject character = Instantiate(asyncOperationHandle.Result);
                    character.transform.position = SceneConfig.instance.player_pos.position;
                    hp = max_hp = 100;
                    OnHPChange?.Invoke(hp, max_hp);
                }
                else
                {
                    Debug.LogError("Failed to load the character prefab.");
                }
            };
        yield return new WaitForSeconds(1);
    }
}
