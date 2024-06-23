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

    float lerpTimer;

    private void Start()
    {
        origHealth = maxHealth;
        currHealth = maxHealth;
    }

    void Update()
    {
        if(GameManager.instance.lerpHPBar.fillAmount != (float)currHealth / origHealth || GameManager.instance.playerHPBar.fillAmount != (float)currHealth / origHealth)
        {
            updateHealthUI();
        }
    }

    public void TakeDamage(float dmg)
    {
        currHealth -= dmg;

        //make sure to put in audio to play for getting hurt

        //make sure to play animation or flash red for getting hurt
        lerpTimer = 0;
        updateHealthUI();

        if (currHealth <= 0)
        {
            GameManager.instance.Lose();
        }
    }
    public void TakeKnockback(Vector3 direction, float force)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(direction * force, ForceMode.Impulse);
        }
    }
    void updateHealthUI()
    {
        float backfill = GameManager.instance.lerpHPBar.fillAmount;
        //float frontfill = GameManager.instance.playerHPBar.fillAmount;
        float currentHealth = currHealth / origHealth;

        lerpTimer += Time.deltaTime;
        float delayBarSpeed = lerpTimer / 2f;
        delayBarSpeed = delayBarSpeed * delayBarSpeed;
        if(backfill > currentHealth)
        {
            GameManager.instance.playerHPBar.fillAmount = currentHealth;
            GameManager.instance.lerpHPBar.fillAmount = Mathf.Lerp(backfill,currentHealth, delayBarSpeed);
        }
        

    }
}
