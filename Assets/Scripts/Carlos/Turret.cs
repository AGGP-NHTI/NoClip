using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : EnemyPawn
{
    public Transform target;
    public float fov = 30;
    public float attackRange = 25;
    public float damping = 5;
    public float attackCoolDown = 3;
    public float attackCooldownCounter = 0;
    GameObject SpawnLoc;
    public GameObject SpawnPrefab;
    public List<GameObject> Barrels;
    int BarrelsIndex = 0;

    float movespeed = 5;

    RaycastHit hit;

    public override void Start()
    {
        base.Start();
        SpawnLoc = Barrels[BarrelsIndex];
    }

    public override void Update()
    {
        base.Update();
        RayHit();

        Vector3 targetDir = target.position - transform.position;
        float angle = Vector3.Angle(targetDir, transform.forward);

        if (angle < fov)
        {
            if (targetDir.magnitude > attackRange)
            {
                attackCooldownCounter = 0;
                return;
            }

            print("Target Sighted");
            var rotation = Quaternion.LookRotation(target.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);

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
        rBody.velocity = instance.transform.forward * movespeed;
        rBody.useGravity = false;
        Destroy(instance.gameObject, 15);

        BarrelsIndex++;

        if (BarrelsIndex == 2)
        {
            BarrelsIndex = 0;
        }

        SpawnLoc = Barrels[BarrelsIndex];

    }

    public void RayHit()
    {
        DebugRay();

        if (Physics.Raycast(raycast, out hit))
        {
            Debug.Log("Raycast: " + hit.collider.gameObject.tag);
        }
    }

    public void DebugRay()
    {
        raycast.origin = this.transform.position;
        raycast.direction = this.transform.forward;

        Debug.DrawRay(raycast.origin, raycast.direction * hit.distance, Color.black);
    }
}
