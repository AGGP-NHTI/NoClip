using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : EnemyPawn
{
    public Transform target;
    public float fov = 30;
    public float attackRange = 25;
    public float damping = 5;
    public float attackCoolDown = 3.0f;
    public float attackCooldownCounter = 0;

    RaycastHit hit;

    void Update()
    {
        DebugRay();

        Vector3 targetDir = target.position - transform.position;
        float angle = Vector3.Angle(targetDir, transform.forward);

        if (angle < fov)
        {
            print("Target Sighted");
            var rotation = Quaternion.LookRotation(target.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);

            if (targetDir.magnitude > attackRange)
            {
                attackCooldownCounter = 0;
                return;
            }

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

    public void PerformAttack()
    {



    }

    public void DebugRay()
    {
        raycast.origin = this.transform.position;
        raycast.direction = this.transform.forward;

        Debug.DrawRay(raycast.origin, raycast.direction * hit.distance, Color.black);
    }
}
