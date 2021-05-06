using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranger : EnemyPawn
{
    public bool canShoot = false;
    public float bulletSpeed = 10;
    public float attackCoolDown = 3;
    public float attackCooldownCounter = 0;
    public GameObject SpawnLoc;
    public GameObject SpawnPrefab;

    RaycastHit hit;

    public override void Update()
    {
        base.Update();

        if (pp)
        {
            transform.LookAt(pp.transform);

            RayHit();
        }

        if (canMove)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }

        if (canShoot)
        {

            attackCooldownCounter += Time.deltaTime;
            if (attackCooldownCounter >= attackCoolDown)
            {
                print("Attack Performed");
                attackCooldownCounter = 0;
                PerformAttack();
            }
        }
        else
        {
            attackCooldownCounter = 0;
        }
    }

    public override void PerformAttack()
    {

        GameObject instance = Instantiate(SpawnPrefab, SpawnLoc.transform.position, SpawnLoc.transform.rotation);
        Rigidbody rBody = instance.GetComponent<Rigidbody>();
        rBody.velocity = instance.transform.forward * bulletSpeed;
        rBody.useGravity = false;
        Destroy(instance.gameObject, 15);

    }

    public void RayHit()
    {
        DebugRay();

        if (Physics.Raycast(raycast, out hit))
        {
            Debug.Log("Raycast: " + hit.collider.gameObject.name);

            if (hit.distance <= 10)
            {
                canMove = false;
                canShoot = true;
            }
            if (hit.distance > 20)
            {
                canShoot = false;
                canMove = true;
            }
        }
    }

    public void DebugRay()
    {
        raycast.origin = this.transform.position;
        raycast.direction = this.transform.forward;

        Debug.DrawRay(raycast.origin, raycast.direction * hit.distance, Color.green);
    }
}