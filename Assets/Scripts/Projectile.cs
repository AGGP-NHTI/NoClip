using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Actor {

    public float damageAmount = 10.0f;
    public float movementSpeed = 20f;
    public float lifetime = 2f;
    Rigidbody rb;
    
    public virtual void Start()
    {
        rb = gameObject.AddComponent<Rigidbody>();
        rb.velocity = transform.forward * movementSpeed;
        rb.useGravity = false; 
        Destroy(gameObject, lifetime);

        GameObject.FindObjectsOfType<Actor>();
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        Actor OtherActor = other.gameObject.GetComponentInParent<Actor>();
        if (OtherActor)
        {
            //LOG("Actor Found");
            OtherActor.TakeDamage( this, damageAmount, Owner);
            new DamageEventInfo(typeof(ProjectileDamageType));
        }
        OnDeath(); 
    }

    public virtual void OnDeath()
    {
        Destroy(gameObject);
    }
}
