using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.InputSystem.InputAction;

public class InputManager : BYSingletonMono<InputManager>
{
    public static Vector3 move_Dir;
    public UnityEvent<bool> OnFire, OnReload;
    public UnityEvent OnChangeGun;
    public void OnMove_System(CallbackContext ctx)
    {
        var value = ctx.ReadValue<Vector2>();
        move_Dir = new Vector3(value.x, 0, value.y);
    }
    public void OnChangeGun_System(CallbackContext ctx)
    {
        if (ctx.started)
            OnChangeGun?.Invoke();
    }
    public void OnFireGun_System(CallbackContext ctx)
    {
        if (ctx.performed && WeaponControl.instance.Cur_WP !=null)
            OnFire?.Invoke(true);
        else
            OnFire?.Invoke(false);
    }
    public void OnReload_System(CallbackContext ctx)
    {
        if (ctx.started && WeaponControl.instance.Cur_WP != null)
            OnReload?.Invoke(true);
        else
            OnReload?.Invoke(false);
    }
   
}
