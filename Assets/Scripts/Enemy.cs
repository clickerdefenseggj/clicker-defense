using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public interface I3DClickHandler
{
    void On3DClick();
}

public class Enemy : MonoBehaviour, I3DClickHandler
{
    public int health = 10;
    public NavMeshAgent agent;
    public EnemyType enemyType;
    EnemyTemplate template;

    void Start ()
    {
        //if (agent && App.inst.playerBase)
        //    agent.SetDestination(App.inst.playerBase.transform.position);

        template = GameData.enemyTemplates[enemyType];
    }

    public void Initialize()
    {        
    }
	
	void Update ()
    {
        if (health <= 0 && gameObject)
            Destroy(gameObject);
	}

    // Probably doesn't work on mobile devices, will need raycast
    public void On3DClick()
    {
        health -= 3;
        Debug.Log("Clicked!");
    }
}
