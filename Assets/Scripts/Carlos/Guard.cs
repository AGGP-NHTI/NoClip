using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : EnemyPawn
{
    public bool canMove = false;
    public bool willMove = false;

    RaycastHit hit;

    public override void Update()
    {
        base.Update();
        RayHit();

        if (pp)
        {
            transform.LookAt(pp.transform);
        }

        if (canMove && willMove)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    public void RayHit()
    {
        DebugRay();

        if (Physics.Raycast(raycast, out hit))
        {
            Debug.Log("Raycast: " + hit.collider.gameObject.name);

            if (hit.distance <= 1)
            {
                canMove = false;
            }
            if (hit.distance >= 5)
            {
                canMove = true;
            }
        }
    }

    public void DebugRay()
    {
        raycast.origin = this.transform.position;
        raycast.direction = this.transform.forward;

        Debug.DrawRay(raycast.origin, raycast.direction * hit.distance, Color.blue);
    }
}