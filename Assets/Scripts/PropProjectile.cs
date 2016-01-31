using UnityEngine;
using System.Collections;
using System;

public class PropProjectile : MonoBehaviour
{
    [SerializeField]
    Rigidbody rigidBody;
    float deathTimer = float.PositiveInfinity;

    [NonSerialized] public const float launchDuration = 0.25f;
    [NonSerialized] public float startTime;
    [NonSerialized] public float endTime;
    public Rigidbody rb;
    Vector3 origin;
    Vector3 destination;
    GameObject originLoc;
    GameObject destLoc;
    Vector3 tanStart;
    Vector3 tanEnd;

    public static PropProjectile Create(Vector3 origin, Vector3 destination)
    {
        var prop = App.Create("PropProjectile").GetComponent<PropProjectile>();
        if (prop)
        {
            var ppc = App.Create("PropProjectileCollider").GetComponent<PropProjectileCollider>();
            if (ppc)
            {
                ppc.transform.position = destination;
            }


            prop.gameObject.transform.position = prop.origin = origin;
            prop.destination = destination;
            //// This doesn't work for pause
            prop.startTime = Time.realtimeSinceStartup;
            prop.endTime = Time.realtimeSinceStartup + launchDuration;

            var dir = new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f));
            dir.Normalize();
            float mag = UnityEngine.Random.Range(150.0f, 300.0f);
            prop.rb.AddTorque(dir * mag);

            prop.Launch();
        }

        return prop;
    }

    public void Launch()
    {
        // PHYSICS
        Vector3 dx = destination - origin;

        float airTime = Mathf.Clamp(dx.magnitude * 0.15f, 0.1f, .25f);

        Vector3 planeVelocity = dx / airTime;
        planeVelocity.y = (dx.y - 0.5f * Physics.gravity.y * airTime * airTime) / airTime;

        rigidBody.velocity = planeVelocity;
    }

    void Update()
    {
        deathTimer -= Time.deltaTime;
        if (deathTimer <= 0f)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        float lingerTime = 0.2f;
        if (deathTimer > lingerTime)
        {
            deathTimer = lingerTime;
        }
    }
}
