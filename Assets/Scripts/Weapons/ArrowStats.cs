using MalbersAnimations.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowStats : MonoBehaviour
{

    MProjectile projectile;

    private void Awake()
    {
        projectile = GetComponent<MProjectile>();
    }
    public void OnHit()
    {
        print(projectile.m_collider.name);
    }
}
