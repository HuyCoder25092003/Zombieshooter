using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    public ParticleSystem particleSystem_;
    public void FireHandle()
    {
        particleSystem_.Play();
    }
}
