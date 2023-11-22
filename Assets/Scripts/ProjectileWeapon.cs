using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;



public enum EProjectileType {

    Fire,
    Space,
    Balloon
}


public class ProjectileWeapon : WeaponBase
{
    public EProjectileType eProjectileType;

    [SerializeField] private float force = 12;
    [SerializeField] private Rigidbody Bullet;
    [SerializeField] private Text _text;
        
    
    protected override void Attack(float percent)
    {
       

        print(percent);
        if (InputManager.magazine > 0)
        {
            Ray camRay = InputManager.GetCameraRay();
            Rigidbody rb = Instantiate(Bullet, camRay.origin, transform.rotation);
            rb.AddForce(Mathf.Max(percent, 0.1f) * force * camRay.direction, ForceMode.Impulse);
            InputManager.magazine--;
            Destroy(rb, 5);
        }
    }

    void Update()
    {
        Bullet.isKinematic = false;

        if (InputManager.eswitch == 1)
        {
            eProjectileType = EProjectileType.Fire;
        }

        if (InputManager.eswitch == 2)
        {
            eProjectileType = EProjectileType.Space;
        }
        if (InputManager.eswitch == 3)
        {
            eProjectileType = EProjectileType.Balloon;
        }



        this.force = InputManager.force;
        _text.text = (" Fire Rate: " + attackSpeed + " | Bullets Remaining: " + InputManager.magazine.ToString() + " | Stockpile: " + InputManager.stockpile.ToString());
        if (eProjectileType == EProjectileType.Fire)
        {
            this.force = force + 150;
            Debug.Log("fdfdf");
            Bullet.useGravity = true;
        }

        if (eProjectileType == EProjectileType.Space)
        {
            Bullet.useGravity = false;
           
        }

        if (eProjectileType == EProjectileType.Balloon)
        {
            this.force = 1f;
            Bullet.useGravity = true;
            
        }
    }





}
