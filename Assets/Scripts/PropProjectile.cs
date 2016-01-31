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
        //StartCoroutine(LaunchRoutine());

        // PHYSICS
        Vector3 dx = destination - origin;

        float airTime = Mathf.Clamp(dx.magnitude * 0.15f, 0.1f, 2f);

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
        float lingerTime = 1.5f;
        if (deathTimer > lingerTime)
        {
            deathTimer = lingerTime;
        }
    }

    public IEnumerator LaunchRoutine()
    {
        // Create locators, point them at each other, find tangents
        originLoc = new GameObject();
        originLoc.name = "Origin";
        destLoc = new GameObject();
        destLoc.name = "Destination";
        originLoc.transform.SetParent(transform);
        destLoc.transform.SetParent(transform);
        originLoc.transform.position = origin;
        destLoc.transform.position = destination;

        originLoc.transform.LookAt(destLoc.transform);
        destLoc.transform.LookAt(originLoc.transform);
        tanStart = Quaternion.AngleAxis(-67.5f, originLoc.transform.right) * originLoc.transform.forward * 15.0f;
        tanEnd = Quaternion.AngleAxis(-67.5f, destLoc.transform.right) * destLoc.transform.forward;

        while (Time.realtimeSinceStartup <= endTime)
        {
            float interpolant = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / launchDuration);

            transform.position = App.GetHermiteCurvePoint(interpolant, origin, destination, tanStart, tanEnd);
            yield return null;
        }

        //Destroy(gameObject);
    }
}
