using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponHandle
{
    void Init(WeaponBehaviour wp);
    void FireHandle();

    void ReloadHandle();
}
