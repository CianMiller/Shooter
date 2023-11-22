using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("Weapon Base Stats")]
    [SerializeField]  public float attackSpeed;
    [SerializeField] protected float chargeTime;
    [SerializeField, Range(0f, 1f)] protected float _minChargePercent;
    [SerializeField] public bool isFullAuto;
    private bool _isOnCooldown;
    private WaitForSeconds _cooldownWait;
    private Coroutine currentFireTimer;
    private WaitUntil _coolDownEnforce;
    private float currentChargeTime;
    
    private void Start()
    {
        _cooldownWait = new WaitForSeconds(attackSpeed);
        _coolDownEnforce = new WaitUntil(() => !_isOnCooldown);
    }

   
    public void StartShooting()
    {
        currentFireTimer = StartCoroutine(ReFireTimer());
    }

    public void StopShooting()
    {
        StopCoroutine(currentFireTimer);
        float percent = currentChargeTime / chargeTime;
        if(percent!=0) { TryAttack(percent); }
    }

    

    private IEnumerator CoolDownTimer()
    {
        _isOnCooldown = true;
        yield return _cooldownWait;
        _isOnCooldown = false;
    }

    private IEnumerator ReFireTimer() 
    {
        print("waiting");
        yield return _coolDownEnforce;
        print("post");

        while(currentChargeTime < chargeTime)
        {
            currentChargeTime += Time.deltaTime;
            yield return null;
        }






        TryAttack(1);
        yield return null; 
        
    }


    protected abstract void Attack(float percent);
    private void TryAttack(float percent)
    {
        currentChargeTime = 0;
        if (!CanAttack(percent)) return;

        Attack(percent);

        StartCoroutine(CoolDownTimer());
        if (isFullAuto && percent>=1)
        {
            currentFireTimer = StartCoroutine(ReFireTimer());
        }

    }

    protected virtual bool CanAttack(float percent)
    {
        return !_isOnCooldown && percent >= _minChargePercent;
    }
}
