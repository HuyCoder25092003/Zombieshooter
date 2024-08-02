using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeView : BaseView
{
    public override void Setup(ViewParam param)
    {
        base.Setup(param);
    }
    public void PlayGame(int id)
    {
        ConfigMissionRecord cf_mission = ConfigManager.instance.configMission.GetRecordBykeySearch(id);
        GameManager.instance.cur_cf_mission = cf_mission;
        LoadSceneManager.instance.LoadSceneByName(cf_mission.SceneName, () =>
        {
            ViewManager.instance.SwitchView(ViewIndex.IngameView);
        });
    }
}
