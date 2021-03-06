﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public interface I3DClickHandler
{
    void On3DClick();
}

public class Enemy : MonoBehaviour
{
    float CurrentHealth = 10;
    public NavMeshAgent agent;
    public EnemyType EnemyType;
    EnemyTemplate Template;

    public float CurrentAttackTimer = 0;
    Vector3 AttackTarget;
    public Animator animator;

    float MaxAttackRangeSquared = 25.0f;
    int lastAttackIndex;
    public int CashValue = 10;
    bool isDead = false;
    public Vector3 destination;

    void Start ()
    {
        // #debug
#if UNITY_EDITOR
        string templateName = "Unnamed";
        if (Template != null)
            templateName = Template.name;
        Debug.Log(templateName + " was spawned at pos = " + transform.position);
#endif

        // #debug
        if (agent)
        {
            agent.enabled = true;
            agent.SetDestination(destination);
        }


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
        PlayWalk(true);
    }
	
	void Update ()
    {
        if (isDead)
            return;

        // Check if dead
        if (CurrentHealth <= 0 && gameObject)
        {
            Player.Inst.NumberKilled++;
            SpawnWaveController.EnemiesKilledThisWave++;
            Player.Inst.AddScore(Template.scoreValue);
            Player.Inst.AddCash(CashValue);

            PlayDeath();
            WaitToDie();
            isDead = true;
            agent.enabled = false;
        }

        // Attack player
        if (Vector3.SqrMagnitude(AttackTarget - transform.position) <= MaxAttackRangeSquared)
        {
            PlayWalk(false);

            CurrentAttackTimer -= Time.deltaTime;

            if (CurrentAttackTimer <= 0)
            {
                Player.Inst.TakeDamage(Template.damage);
                CurrentAttackTimer = Template.attackRate;
                PlayAttack();
            }
        }
    }

    void ApplyDamage(float damage)
    {
        if (isDead)
            return;

        CurrentHealth -= damage;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (isDead)
            return;

        if (other)
        {
            var ppc = other.gameObject.GetComponent<PropProjectileCollider>();
            if (ppc)
            {
                //Hit();
            }
        }
    }

    public void Hit(PropProjectile prop, float damage = 100)
    {
        if (isDead)
            return;

        ApplyDamage(damage + Player.Inst.DamageBonus);

        // Stun coroutine
        StartCoroutine(Stun(1));
    }

    public IEnumerator Stun(float stunDuration)
    {
        if (isDead)
            yield break;

        agent.speed = 0.0f;
        PlayWalk(false);
        yield return new WaitForSeconds(stunDuration);

        // wait for the game to start running again before we unstun
        yield return new WaitUntil(App.inst.GetIsRunning);

        agent.speed = Template.speed;
        PlayWalk(true);
    }

    public void WaitToDie()
    {
        StartCoroutine(WaitToDieRoutine());
    }

    public IEnumerator WaitToDieRoutine()
    {
        if (isDead)
            yield break;

        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);

        // Fade animation instead of purely destroy call?
    }

    public void Pause()
    {
        if (isDead)
            return;

        agent.speed = 0.0f;
        PlayWalk(false);
    }

    public void UnPause()
    {
        if (isDead)
            return;

        agent.speed = Template.speed;
        PlayWalk(true);
    }

    public void PlayWalk(bool walk)
    {
        if (isDead)
            return;

        if (animator)
        {
            if (walk)
            {
                animator.SetFloat("Velocity", Mathf.Pow(Template.speed, 15.0f));
            }
            else
            {
                animator.SetFloat("Velocity", 0.0f);
            }
        }
    }

    public void PlayAttack()
    {
        if (isDead)
            return;

        if (animator)
        {
            int attackIndex = UnityEngine.Random.Range(1, 5);

            if (attackIndex == lastAttackIndex)
            {
                if (attackIndex < 4)
                    ++attackIndex;
                else if (attackIndex == 4)
                    --attackIndex;
            }

            int soundIndex = UnityEngine.Random.Range(1, 3);
            SoundManager.PlaySfx("sfx/sword_slash_" + soundIndex);


            animator.SetBool("Attack" + attackIndex, true);
            lastAttackIndex = attackIndex;
        }
    }

    public void PlayDeath()
    {
        if (isDead)
            return;

        if (animator)
        {
            int deathIndex = UnityEngine.Random.Range(1, 3);
            animator.SetBool("Death" + deathIndex, true);
        }

        int soundIndex = UnityEngine.Random.Range(1, 4);
        SoundManager.PlaySfx("sfx/death_grunt_" + soundIndex);

        if (agent)
            agent.speed = 0.0f;
    }

    public void PlayHurt()
    {
        if (isDead)
            return;
    }

}
