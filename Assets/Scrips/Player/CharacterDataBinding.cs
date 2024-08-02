using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterDataBinding : MonoBehaviour
{
    [SerializeField]Animator animator;
    public Vector3 MoveDir
    {
        set
        {
            animator.SetFloat(key_Anim_X, value.x);

            animator.SetFloat(key_Anim_Y, value.z);
        }
    }
    private int key_Anim_X;
    private int key_Anim_Y;
    void Start()
    {
        key_Anim_X = Animator.StringToHash("X");
        key_Anim_Y = Animator.StringToHash("Y");
        WeaponControl.instance.OnChangeGun.AddListener(OnChangeGun);
    }
    public void PlayShowGun()
    {
        animator.Play("Draw", 0, 0);
    }
    public void PlayFireGun()
    {
        animator.Play("Fire", 1, 0);
    }
    public void PlayReloadGun()
    {
        animator.Play("Reload", 1, 0);
    }
    public void SetSpeedReload(float speed)
    {
        animator.SetFloat("SpeedReload", speed);
    }
    private void OnChangeGun(WeaponBehaviour wp)
    {
        animator.runtimeAnimatorController = wp.overrideController;
    }
}
