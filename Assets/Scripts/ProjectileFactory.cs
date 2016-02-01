using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileFactory : MonoBehaviour
{
    private static ProjectileFactory m_Inst;
    public static ProjectileFactory inst { get { return m_Inst; } }

    [SerializeField]
    public PropProjectile cannonball;

    [SerializeField]
    public List<PropProjectile> prefabs = new List<PropProjectile>();

    void OnEnable()
    {
        if (!m_Inst)
            m_Inst = this;
        else if (m_Inst != this)
            Destroy(gameObject);
    }

    public PropProjectile InstantiateRandom()
    {
        return Instantiate(prefabs[Random.Range(0, prefabs.Count)]);
    }

    public PropProjectile InstantiateCannonball()
    {
        return Instantiate(cannonball);
    }
}
