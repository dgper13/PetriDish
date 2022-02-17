using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public HealthStatus status;
    public float CurrentHealth;
    public float MaxHealth = 100f;
    public int HealingTime;
    public int SickDays;
    public int MinHealingTime;
    public int MaxHealingTime;
    public int MinSeverity;
    public int MaxSeverity;

    void Start()
    {
        CurrentHealth = MaxHealth;
        HealingTime = Random.Range(MinHealingTime, MaxHealingTime);
        SickDays = 0;
    }

    public void Step()
    {
        if (status != HealthStatus.Dead)
        {
            if (status == HealthStatus.Infected)
            {
                CurrentHealth -= Random.Range(MinSeverity, MaxSeverity);
                this.GetComponent<SpriteRenderer>().color = Color.red;
                SickDays += 1;
            }

            else if (status == HealthStatus.Immune)
            {
                this.GetComponent<SpriteRenderer>().color = Color.green;
            }

            if (CurrentHealth <= 0f)
            {
                status = HealthStatus.Dead;
                CurrentHealth = 0f;
                this.GetComponent<SpriteRenderer>().color = Color.black;
            }
            
            else
            {
                if (SickDays == HealingTime)
                {
                    SickDays = 0;
                    status = HealthStatus.Immune;
                    this.GetComponent<SpriteRenderer>().color = Color.green;
                }
            }
        }
    }
}