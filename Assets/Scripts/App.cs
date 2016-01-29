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
	
	}

  

    public static GameObject Create(string prefabName)
    {
        if (!inst.cachedObjects.ContainsKey(prefabName))
        {
            inst.cachedObjects[prefabName] = Resources.Load(prefabName);
        }

        return Instantiate(inst.cachedObjects[prefabName]) as GameObject;
    }

    void FixedUpdate()
    {
        //if mouse button (left hand side) pressed instantiate a raycast
        //if (Input.GetMouseButtonDown(0))
        //{
        //    //create a ray cast and set it to the mouses cursor position in game
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;
        //    if (Physics.Raycast(ray, out hit, testRaycastDistance))
        //    {
        //        //draw invisible ray cast/vector
        //        Debug.DrawLine(ray.origin, hit.point);
        //        //log hit area to the console
        //        Debug.Log(hit.point);

        //        agent.SetDestination(hit.point);
        //    }
        //}

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
