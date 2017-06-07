using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTarget : MonoBehaviour, IDamageTarget {

    public int health = 20;
    public bool dead;
    int damageFactor;

    public void OnStartReceivingDamage(int damage)
    {
        damageFactor = damage;
        InvokeRepeating("OnReceiveDamage", 0f, 1f);
    }

    public void OnStopReceivingDamage()
    {
        CancelInvoke("OnReceiveDamage");
        print("Yay!");
    }

    protected void OnReceiveDamage()
    {
        health -= damageFactor;
        if (health <= 0)
        {
            dead = true;
        }
    }
}
