using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : EnemyPawn
{
    public Transform target;

    void Update()
    {
        Vector3 targetDir = target.position - transform.position;
        float angle = Vector3.Angle(targetDir, transform.forward);

        if (angle < 30.0f || angle < -30.0f)
        {
            print("close");
        }
    }
}
