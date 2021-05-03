using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : EnemyPawn
{
    public GameObject MC;
    public GameObject Guardian;

    public bool canMove = false;
    public float speed = 3;

    RaycastHit hit;

    void Start()
    {
        Vector3 startPos = gameObject.transform.position;
    }

    public override void Update()
    {
        base.Update();

        if (MC)
        {
            transform.LookAt(MC.transform);

            RayHit();
        }

        if (canMove)
        {
            Guardian.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    public void RayHit()
    {
        DebugRay();

        if (Physics.Raycast(raycast, out hit))
        {
            Debug.Log("Raycast: " + hit.collider.gameObject.name);
        }
    }

    public void DebugRay()
    {
        raycast.origin = this.transform.position;
        raycast.direction = this.transform.forward;

        Debug.DrawRay(raycast.origin, raycast.direction * hit.distance, Color.blue);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other == MC)
        {
            canMove = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other == MC)
        {
            canMove = false;
        }
    }
}