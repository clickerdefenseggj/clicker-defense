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

    public SpawnWaveController SpawnController;

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
        // DEBUG: Testing leaderboard. Add one score per update.
        if (IsRunning)
        {
            //Player.Inst.AddScore(1);

            //if mouse button(left hand side) pressed instantiate a raycast
            if (Input.GetMouseButtonDown(0))
            { // if left button pressed...
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider)
                    {
                        var clickHandler = hit.collider.gameObject.GetComponent<I3DClickHandler>();
                        if (clickHandler != null)
                            clickHandler.On3DClick();
                    }

                    // the object identified by hit.transform was clicked
                    // do whatever you want
                }
            }

        }
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

        new GameSparks.Api.Requests.LogEventRequest().SetEventKey("SUBMIT_SCORE").SetEventAttribute("SCORE", Player.Inst.GetScore()).Send((response) => {
            if (!response.HasErrors)
            {
                Debug.Log("Score Posted Successfully...");
            }
            else {
                Debug.Log("Error Posting Score...");
            }
        });

        App.inst.IsRunning = false;

        SetManager.OpenSet<LeaderboardSet>();
    }
}
