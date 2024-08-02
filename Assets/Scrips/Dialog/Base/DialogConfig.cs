using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogIndex
{
    WinDialog = 1,
    FailDialog = 2

}
public class DialogParam
{

}
public class WinDialogParam : DialogParam
{
    public ConfigMissionRecord cf_mission;
}
public class DialogConfig 
{
    public static DialogIndex[] dialogIndices =
    {
        DialogIndex.WinDialog,
        DialogIndex.FailDialog,  
    };
}
