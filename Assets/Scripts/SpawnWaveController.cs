using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnWaveController : MonoBehaviour {

    const int NUM_LANES = 4;

    public GameObject[] SpawnLocations = new GameObject[NUM_LANES];
    public GameObject[] MovementDestinations = new GameObject[NUM_LANES];

    public int CurrentWave = 1;

    float MinSpawnFrequencyMin = 1;
    float MinSpawnFrequencyMax = 7;
    float MaxSpawnFrequencyMin = 8;
    float MaxSpawnFrequencyMax = 10;
    float MinSpawnTime = 1f;

    float NumEnemiesPerWaveIncrease = 5; 

    float CurrentSpawnTime = 0;
    float NextSpawnTime = 0;

    public static int EnemiesSpawnedThisWave = 0;
    public static int EnemiesKilledThisWave = 0;

    List<Enemy> SpawnedEnemies = new List<Enemy>();

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {

        if (App.inst.IsRunning)
        {
            if (EnemiesKilledThisWave < CurrentWave * NumEnemiesPerWaveIncrease)
            {
                if(NextSpawnTime == 0)
                    ChooseNextSpawnTime();

                CurrentSpawnTime += Time.deltaTime;

                if (CurrentSpawnTime > NextSpawnTime && EnemiesSpawnedThisWave < CurrentWave * NumEnemiesPerWaveIncrease)
                {
                    // Pick a random template
                    EnemyType template;
                    
                    if(CurrentWave < 3)
                        template = (EnemyType)Random.Range(0, CurrentWave);
                    else
                        template = (EnemyType)Random.Range(0, GameData.enemyTemplates.Count);

                    // Pick a random lane
                    int lane = Random.Range(0, NUM_LANES);

                    if (lane < SpawnLocations.Length && SpawnLocations[lane] != null)
                    {
                        // Spawn an enemy
                        GameObject enemyGO = App.Create("Units/" + GameData.enemyTemplates[template].name + "/" + GameData.enemyTemplates[template].name);

                        if (!enemyGO)
                        {
                            Debug.Log("Failed to find prefab: " + GameData.enemyTemplates[0].name);
                            return;
                        }
                        else
                        {
                            enemyGO.transform.position = SpawnLocations[lane].transform.position;
                            Debug.Log("Successfully created enemy: " + GameData.enemyTemplates[template].name + " at pos = " + SpawnLocations[lane].transform.position);

                            Enemy newEnemy = enemyGO.GetComponent<Enemy>();
                            if (newEnemy)
                                newEnemy.Initialize(template, MovementDestinations[lane].transform.position);
                            else
                                Debug.Log("No Enemy component on: " + enemyGO);

                            NavMeshAgent newAgent = enemyGO.GetComponent<NavMeshAgent>();
                            if (newAgent)
                            {
                                newEnemy.destination = MovementDestinations[lane].transform.position;
                                //newAgent.SetDestination(MovementDestinations[lane].transform.position);
                            }

                            EnemiesSpawnedThisWave++;
                            SpawnedEnemies.Add(newEnemy);
                        }
                    }
                    // Reset spawn timer
                    CurrentSpawnTime = 0;
                    ChooseNextSpawnTime();
                }
            }
            else
            {
                WaveCompleted();
            }
        }


	}

    public void WaveCompleted()
    {
        SoundManager.PlayClip("sfx/wave_won");

        CurrentWave++;
        EnemiesKilledThisWave = 0;
        EnemiesSpawnedThisWave = 0;
        SetManager.OpenSet<StoreSet>();
    }

    public void Reset()
    {
        ClearEnemies();

        EnemiesKilledThisWave = 0;
        EnemiesSpawnedThisWave = 0;
        CurrentSpawnTime = 0;
        NextSpawnTime = 0;
    }

    public void ClearEnemies()
    {
        foreach (Enemy enemy in SpawnedEnemies)
        {
            if (enemy)
                Destroy(enemy.gameObject);
        }
        SpawnedEnemies.Clear();
    }

    void ChooseNextSpawnTime()
    {
        NextSpawnTime = Random.Range(Random.Range(MinSpawnFrequencyMin, MinSpawnFrequencyMax), Mathf.Min(Random.Range(MaxSpawnFrequencyMin, MaxSpawnFrequencyMax) - (CurrentWave * 0.1f), MinSpawnTime));
    }

    public void PauseEnemiesForCinematic()
    {
        print("(zesty): Pausing " + SpawnedEnemies.Count + " enemies");
        App.inst.IsRunning = false;
        foreach (Enemy CurrentEnemy in SpawnedEnemies)
        {
            CurrentEnemy.Pause();
        }
    }

    public void UnpauseEnemiesAfterCinematic()
    {
        ChooseNextSpawnTime();
        App.inst.IsRunning = true;
        foreach (Enemy CurrentEnemy in SpawnedEnemies)
        {
            CurrentEnemy.UnPause();
        }
    }
}
