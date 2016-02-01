using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class App : MonoBehaviour
{
    public static App inst { get { return m_inst;  } }
    static App m_inst;

    Dictionary<string, UnityEngine.Object> cachedObjects = new Dictionary<string, UnityEngine.Object>();

    public float testRaycastDistance = 50.0f;
    public NavMeshAgent agent;
    public static GameplaySet gameplaySet;

    public GameObject playerBase;

    public static void PauseGameplayMusic()
    {
        if (currBgm != null)
            currBgm.Stop();
    }

    public Transform projectileSpawnPoint;

    public LayerMask clickLayerMask;
    public SpawnWaveController SpawnController;
    public Skybox GameplaySkybox;

    const int NUM_SKYBOXES = 5;
    public Material[] SkyboxMaterials = new Material[NUM_SKYBOXES];
    public Light[] DirectionalLights = new Light[NUM_SKYBOXES];


    float CurrentRandomExclamationTime = 0;
    float NextExclamationTime = 0;
    float RandomExclamationMin = 3;
    float RandomExclamationMax = 10;

    public static void PlayGameplayMusic()
    {
        if (currBgm != null)
        {
            currBgm.Play();
        }
        else
        {
            currBgm = SoundManager.PlayBgm("bgm/gameplay_music");
        }
    }

    int PreviousSkyboxNumber = -1;

    public bool IsRunning = false;
        
    public static AudioSource currBgm;

    public bool UseCannonball = true;

    void Awake()
    {
        if (m_inst == null)
            m_inst = this;
    }

	// Game entry point
	void Start ()
    {
        currBgm = SoundManager.PlayBgm("bgm/menu_music");
        SetManager.OpenSet<MainMenuSet>();

        NextExclamationTime = UnityEngine.Random.Range(RandomExclamationMin, RandomExclamationMax);
    }
	
	// Update is called once per frame
	void Update ()
    {
        SoundManager.Update();
       
        if (IsRunning && Input.GetMouseButtonDown(0))
        { // if left button pressed...
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickLayerMask))
            {
                if (hit.collider)
                {
                    // Notify enemies that they have been clicked
                    var clickHandler = hit.collider.gameObject.GetComponent<I3DClickHandler>();
                    if (clickHandler != null)
                        clickHandler.On3DClick();

                    // Create the projectile
                    if (playerBase)
                        PropProjectile.Create(projectileSpawnPoint.position, hit.point);

                }

                // the object identified by hit.transform was clicked
                // do whatever you want
            }
        }



        if (IsRunning && !UseCannonball)
        {
            CurrentRandomExclamationTime += Time.deltaTime;

            if (CurrentRandomExclamationTime >= NextExclamationTime)
            {
                CurrentRandomExclamationTime = 0;

                CinematicSet cs = SetManager.OpenSet<CinematicSet>();
                cs.BeginCinematic(CinematicSet.Type.RandomExclamation);

                NextExclamationTime = UnityEngine.Random.Range(RandomExclamationMin, RandomExclamationMax);
            }
        }


        // ------------------------
        // Zesty Dev Hacks
        // ------------------------
        if (Input.GetKeyDown(KeyCode.C))
        {
            print("(zesty) Testing conversation...");
            CinematicSet cs = SetManager.OpenSet<CinematicSet>();
            cs.BeginCinematic(CinematicSet.Type.HoarderConversation);
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            CinematicSet cs = SetManager.OpenSet<CinematicSet>();
            cs.BeginCinematic(CinematicSet.Type.RandomExclamation);
        }
        // ------------------------

#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.Space))
            Time.timeScale = 5.0f;
        else
            Time.timeScale = 1.0f;
#endif
    }  

    public static GameObject Create(string prefabName)
    {
        if (!inst.cachedObjects.ContainsKey(prefabName))
        {
            inst.cachedObjects[prefabName] = Resources.Load(prefabName);
        }

        return Instantiate(inst.cachedObjects[prefabName]) as GameObject;
    }

    public void EndLevel()
    {
        PauseGameplayMusic();

        if (currBgm)
            currBgm.Stop();
        currBgm = SoundManager.PlayBgm("bgm/menu_music");

        if (App.inst.IsRunning == false)
            return;


        new GameSparks.Api.Requests.LogEventRequest().SetEventKey("SUBMIT_SCORE").SetEventAttribute("SCORE", Player.Inst.GetScore()).Send((response) =>
        {
            //        agent.SetDestination(hit.point);
            if (!response.HasErrors)
            {
                Debug.Log("Score Posted Successfully...");
            }
            else
            {
                Debug.Log("Error Posting Score...");
            }
        });

        App.inst.IsRunning = false;

        SetManager.OpenSet<LeaderboardSet>();
    }

    public static Vector3 GetHermiteCurvePoint(float t, Vector3 start, Vector3 end, Vector3 tanStart, Vector3 tanEnd)
    {
        Vector3 point = ((1.0f - (3.0f * Mathf.Pow(t, 2.0f)) + (2.0f * Mathf.Pow(t, 3.0f))) * start)
            + (Mathf.Pow(t, 2) * (3.0f - (2.0f * t)) * end)
            + ((t * Mathf.Pow((t - 1.0f), 2.0f)) * tanStart)
            + (Mathf.Pow(t, 2.0f) * (t - 1.0f) * tanEnd);
        return point;


    }

    public void ChooseRanomSkybox()
    {
        if(GameplaySkybox)
        {
            int newSkybox = 0;

            if (PreviousSkyboxNumber != -1)
            {
                //newSkybox = UnityEngine.Random.Range(0, NUM_SKYBOXES);

                newSkybox = (SpawnController.CurrentWave % 5) - 1;

                // never pick the same skybox twice
                while (newSkybox == PreviousSkyboxNumber)
                {
                    newSkybox = UnityEngine.Random.Range(0, NUM_SKYBOXES);
                }
            }

            if (PreviousSkyboxNumber != -1)
                DirectionalLights[PreviousSkyboxNumber].gameObject.SetActive(false);

            DirectionalLights[newSkybox].gameObject.SetActive(true);

            GameplaySkybox.material = SkyboxMaterials[newSkybox];
            PreviousSkyboxNumber = newSkybox;
        }
    }

    public void PlayLevel1Cinematic()
    {
        StartCoroutine(PlayLevel1CinematicCoroutine());

    }

    public IEnumerator PlayLevel1CinematicCoroutine()
    {
        yield return new WaitForSeconds(6);

        CinematicSet cs = SetManager.OpenSet<CinematicSet>();
        cs.BeginCinematic(CinematicSet.Type.HoarderConversation);
        UseCannonball = false;
    }

}
