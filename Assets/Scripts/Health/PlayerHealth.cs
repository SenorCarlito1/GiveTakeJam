using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamage
{
    [Header("----Player Health----")]
    [SerializeField] public float maxHealth;
    public float currHealth;
    private float origHealth;

    private void Start()
    {
        origHealth = maxHealth;
        currHealth = maxHealth;
    }

    public void TakeDamage(float dmg)
    {
        currHealth -= dmg;

        //make sure to put in audio to play for getting hurt

        //make sure to play animation or flash red for getting hurt

        if (currHealth <= 0)
        {
            GameManager.instance.Lose();
        }
    }
}
