using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : EnemyPawn
{
    public GameObject MC;

    public bool canMove = true;
    public float speed = 20;

    RaycastHit hit;

    void Update()
    {
        if (MC)
        {
            transform.LookAt(MC.transform);

            DebugRay();
            RayHit();
        }

        if (canMove)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    public void RayHit()
    {
        if (Physics.Raycast(raycast, out hit))
        {
            Debug.Log("Raycast: " + hit.collider.gameObject.name);

            if (hit.distance <= 10)
            {
                //speed = 3;
            }
            if (hit.distance <= 1)
            {
                canMove = false;
            }
            if (hit.distance > 10)
            {
                canMove = true;
                speed = 10;
            }
        }
    }

    public void DebugRay()
    {
        raycast.origin = this.transform.position;
        raycast.direction = this.transform.forward;

        Debug.DrawRay(raycast.origin, raycast.direction * 100, Color.red);
    }
}
