using MalbersAnimations;
using MalbersAnimations.Controller;
using MalbersAnimations.Controller.AI;
using MalbersAnimations.Utilities;
using MalbersAnimations.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TurretAI : MonoBehaviour
{
    Aim aim;
   
    public float radius;

    public float rate;
    float reset;

    public LayerMask layerMask;

    public GameObject bullet;
    public Transform node;
    public GameObject muzzleFlash;
    private void Awake()
    {
        aim = GetComponent<Aim>();  
        
    }

    private void Start()
    {
        reset = rate;
    }

    private void Update()
    {
        rate -= Time.deltaTime;
        if(rate < 0)
        {
            rate = reset;
            AimAndShoot();
        }
    }

    void AimAndShoot()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, radius, layerMask);

        for (int i = 0; i < cols.Length; i++)
        {
            //if we hit Enemy and it's not dead
            if (cols[i] != null)
            {
                if (cols[i].GetComponent<MAnimal>() != null)
                {
                    MAnimal animal = cols[i].GetComponent<MAnimal>();
                    if (animal.enabled == true)
                    {
                        aim.SetTarget(cols[i].transform);
                        GameObject _bullet = Instantiate(bullet, node.position, node.rotation);
                        _bullet.GetComponent<MProjectile>().Fire(node.forward * 15f);
                        _bullet.GetComponent<AudioSource>().Play();
                        muzzleFlash.SetActive(true);
                        break;
                    }
                }
               
            }
        }

    }
}
