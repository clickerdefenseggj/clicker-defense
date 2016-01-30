using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SetManager : MonoBehaviour {

    public static SetManager Inst { get { return m_Inst; } }
    static SetManager m_Inst;

    public Set currentSet;

    [SerializeField]
    List<Set> Sets = new List<Set>();

    void OnEnable()
    {
        if (!m_Inst)
        {
            m_Inst = this;
            DontDestroyOnLoad(this);
        }
        else if (m_Inst != this)
        {
            Destroy(gameObject);
        }
    }

    public static T OpenSet<T>() where T : Set
    {
        string setName = typeof(T).Name;

        // Make current set inactive
        if (Inst.currentSet != null) { Inst.currentSet.gameObject.SetActive(true); }

        // Create the new set and mark active
        GameObject setGO = App.Create(setName);
        if (setGO != null)
        {
            T castedSet = setGO.GetComponent<T>();
            if (castedSet != null)
            {
                Inst.Sets.Add(castedSet);
                Inst.currentSet = castedSet;
                Inst.currentSet.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning(setName + " didn't have a Set component attached, adding it for you..");
                castedSet = setGO.AddComponent<T>();
                Inst.Sets.Add(castedSet);
                Inst.currentSet = castedSet;
                Inst.currentSet.gameObject.SetActive(true);
            }

            return castedSet;
        }

        return null;
    }

    public static void CloseSet(Set set)
    {
        var inst = Inst;
        if(!inst.Sets.Contains(set))
        {
            Debug.Log("Tried to close a set that wasn't on the stack");
            return;
        }

        RemoveAndDestroySet(set);
    }

    static void RemoveAndDestroySet(Set set)
    {
        Inst.Sets.RemoveAll(s => (object)s == set);

        if(set)
        {
            Destroy(set.gameObject);
        }
    }
}
