using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class App : MonoBehaviour
{
    public static App inst { get { return m_inst;  } }
    static App m_inst;

    public Set currentSet;

    List<Set> sets = new List<Set>();
    Dictionary<string, UnityEngine.Object> cachedObjects = new Dictionary<string, UnityEngine.Object>();

    void Awake()
    {
        if (m_inst == null)
            m_inst = this;

        //OpenSet<MainMenuSet>();
    }

	// Game entry point
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public static T OpenSet<T>() where T : Set
    {
        string setName = typeof(T).Name;

        // Make current set inactive
        if (inst.currentSet != null) { inst.currentSet.gameObject.SetActive(true); }

        // Create the new set and mark active
        GameObject setGO = Create(setName);
        if (setGO != null)
        {
            T castedSet = setGO.GetComponent<T>();
            if (castedSet != null)
            {
                inst.sets.Add(castedSet);
                inst.currentSet = castedSet;
                inst.currentSet.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning(setName + " didn't have a Set component attached, adding it for you..");
                castedSet = setGO.AddComponent<T>();
                inst.sets.Add(castedSet);
                inst.currentSet = castedSet;
                inst.currentSet.gameObject.SetActive(true);
            }

            return castedSet;
        }

        return null;
    }

    public static GameObject Create(string prefabName)
    {
        if (!inst.cachedObjects.ContainsKey(prefabName))
        {
            inst.cachedObjects[prefabName] = Resources.Load(prefabName);
        }

        return Instantiate(inst.cachedObjects[prefabName]) as GameObject;
    }

    public float distance = 50.0f;
    public NavMeshAgent agent;

    void FixedUpdate()
    {
        //if mouse button (left hand side) pressed instantiate a raycast
        if (Input.GetMouseButtonDown(0))
        {
            //create a ray cast and set it to the mouses cursor position in game
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distance))
            {
                //draw invisible ray cast/vector
                Debug.DrawLine(ray.origin, hit.point);
                //log hit area to the console
                Debug.Log(hit.point);

                agent.SetDestination(hit.point);
            }
        }
    }
}
