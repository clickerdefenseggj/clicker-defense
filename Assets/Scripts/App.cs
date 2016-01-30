using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class App : MonoBehaviour
{
    public static App inst { get { return m_inst;  } }
    static App m_inst;

    Dictionary<string, UnityEngine.Object> cachedObjects = new Dictionary<string, UnityEngine.Object>();

    public float testRaycastDistance = 50.0f;
    public NavMeshAgent agent;

    public GameObject playerBase;

    public LayerMask clickLayerMask;
    public SpawnWaveController SpawnController;
    public Skybox GameplaySkybox;

    const int NUM_SKYBOXES = 5;
    public Material[] SkyboxMaterials = new Material[NUM_SKYBOXES];
    int PreviousSkyboxNumber = -1;

    public bool IsRunning = false;

    void Awake()
    {
        if (m_inst == null)
            m_inst = this;
    }

	// Game entry point
	void Start ()
    {
        SetManager.OpenSet<MainMenuSet>();
    }
	
	// Update is called once per frame
	void Update ()
    {
       
        if (Input.GetMouseButtonDown(0))
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
                        PropProjectile.Create(playerBase.transform.position, hit.point);

                }

                // the object identified by hit.transform was clicked
                // do whatever you want
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
            int newSkybox = Random.Range(0, NUM_SKYBOXES);

            // never pick the same skybox twice
            while(newSkybox == PreviousSkyboxNumber)
            {
                newSkybox = Random.Range(0, NUM_SKYBOXES);
            }

            GameplaySkybox.material = SkyboxMaterials[newSkybox];
            PreviousSkyboxNumber = newSkybox;
        }
    }

}
