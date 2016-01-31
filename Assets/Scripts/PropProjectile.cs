using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class PropProjectile : MonoBehaviour
{
    public int DamageValue = 100;

    new Rigidbody rigidbody;

    Vector3 origin;
    Vector3 destination;
    float deathTimer = float.PositiveInfinity;

    private HashSet<Enemy> damaged = new HashSet<Enemy>();

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public static PropProjectile Create(Vector3 origin, Vector3 destination)
    {
        var prop = ProjectileFactory.inst.InstantiateRandom();
        //var prop = App.Create("PropProjectile").GetComponent<PropProjectile>();
        if (prop)
        {
            //var ppc = App.Create("PropProjectileCollider").GetComponent<PropProjectileCollider>();
            //if (ppc)
            //{
            //    ppc.transform.position = destination;
            //}

            prop.gameObject.transform.position = prop.origin = origin;
            prop.destination = destination;
            //// This doesn't work for pause
            //prop.startTime = Time.realtimeSinceStartup;
            //prop.endTime = Time.realtimeSinceStartup + launchDuration;

            var dir = new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f));
            dir.Normalize();
            float mag = UnityEngine.Random.Range(150.0f, 300.0f);
            prop.rigidbody.AddTorque(dir * mag);

            prop.Launch();
        }

        return prop;
    }

    public void Launch()
    {
        // PHYSICS
        // Calculates initial velocity to form an arc with gravity that goes through destination at airTime
        // So airtime and the value of gravity controls how high it arcs

        Vector3 deltaPosition = destination - origin;
        //Debug.Log(deltaPosition.magnitude);

        float airTime = Mathf.Clamp(deltaPosition.magnitude * 0.025f, 0.1f, 2f);

        // horizontal velcity: just distance by time
        Vector3 velocity = deltaPosition / airTime;
        // vertical velocity: solve gravity parabola (0.5*g*t^2 + v0*t + p0) for v0
        velocity.y = (deltaPosition.y - 0.5f * Physics.gravity.y * airTime * airTime) / airTime;

        rigidbody.velocity = velocity;
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
        float lingerTime = 1f;
        if (deathTimer > lingerTime)
        {
            deathTimer = lingerTime;
        }

        var enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy && !damaged.Contains(enemy))
        {
            damaged.Add(enemy);
            enemy.Hit(DamageValue);
        }
    }
}
