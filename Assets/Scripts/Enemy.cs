﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public interface I3DClickHandler
{
    void On3DClick();
}

public class Enemy : MonoBehaviour, I3DClickHandler
{
    float CurrentHealth = 10;
    public NavMeshAgent agent;
    public EnemyType EnemyType;
    EnemyTemplate Template;

    public float CurrentAttackTimer = 0;
    Vector3 AttackTarget;

    float MaxAttackRangeSquared = 5;

    void Start ()
    {
        //if (agent && App.inst.playerBase)
        //    agent.SetDestination(App.inst.playerBase.transform.position);

    }

    public void Initialize(EnemyType enemyType, Vector3 attackTarget)
    {
        EnemyType = enemyType;
        Template = GameData.enemyTemplates[enemyType];
        CurrentHealth = Template.maxHealth;
        agent.speed = Template.speed;
        AttackTarget = attackTarget;
        CurrentAttackTimer = Template.attackRate;
    }
	
	void Update ()
    {
        if (CurrentHealth <= 0 && gameObject)
        {
            Destroy(gameObject);
            SpawnWaveController.EnemiesKilledThisWave++;
            Player.Inst.AddScore(Template.scoreValue);

        }

        // Attack player
        if (Vector3.SqrMagnitude(AttackTarget - transform.position) <= MaxAttackRangeSquared)
        {
            CurrentAttackTimer -= Time.deltaTime;

            if (CurrentAttackTimer <= 0)
            {
                Player.Inst.TakeDamage(Template.damage);
                CurrentAttackTimer = Template.attackRate;
            }
        }
    }

    void ApplyDamage(float damage)
    {
        CurrentHealth -= damage;
    }

    public void On3DClick()
    {
        ApplyDamage(100);

        // Stun coroutine
        StartCoroutine(Stun(1));
    }

    public IEnumerator Stun(float stunDuration)
    {
        agent.speed = 0.0f;
        yield return new WaitForSeconds(stunDuration);
        agent.speed = Template.speed;
    }

}
