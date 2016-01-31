using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

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

    float MaxAttackRangeSquared = 5;
    int lastAttackIndex;
    public int CashValue = 10;

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
        PlayWalk(true);
    }
	
	void Update ()
    {
        if (CurrentHealth <= 0 && gameObject)
        {
            SpawnWaveController.EnemiesKilledThisWave++;
            Player.Inst.AddScore(Template.scoreValue);
            Player.Inst.Cash += CashValue;

            Destroy(gameObject);
        }

        // Attack player
        if (Vector3.SqrMagnitude(AttackTarget - transform.position) <= MaxAttackRangeSquared)
        {
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
        CurrentHealth -= damage;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other)
        {
            var ppc = other.gameObject.GetComponent<PropProjectileCollider>();
            if (ppc)
            {
                Hit();
            }
        }
    }

    private void Hit()
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
        PlayWalk(false);
    }

    public void Pause()
    {
        agent.speed = 0.0f;
        PlayWalk(false);
    }

    public void UnPause()
    {
        agent.speed = Template.speed;
        PlayWalk(true);
    }

    public void PlayWalk(bool walk)
    {
        if (animator)
        {
            if (walk)
            {
                animator.SetFloat("Velocity", 10.0f * Template.speed);
            }
            else
            {
                animator.SetFloat("Velocity", 0.0f);
            }
        }
    }

    public void PlayAttack()
    {
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

            animator.SetBool("Attack" + attackIndex, true);
            lastAttackIndex = attackIndex;
        }
    }

    public void PlayDeath()
    {
        if (animator)
        {
            int deathIndex = UnityEngine.Random.Range(1, 3);
            animator.SetBool("Death" + deathIndex, true);
        }
    }

    public void PlayHurt()
    {

    }

}
