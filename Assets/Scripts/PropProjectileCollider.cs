using UnityEngine;
using System.Collections;

public class PropProjectileCollider : MonoBehaviour
{
	// Use this for initialization
	void Start()
    {
        StartCoroutine(DestroyAfterFrame());
	}
	
	// Update is called once per frame
	void Update()
    {
	}

    public IEnumerator DestroyAfterFrame()
    {
        yield return null;
        Destroy(gameObject);
    }
}
