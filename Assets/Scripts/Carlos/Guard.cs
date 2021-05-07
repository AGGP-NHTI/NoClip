using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : EnemyPawn
{
    public bool willMove = false;
    public bool canAttack = false;
    public float attackRange = 2;
    public float attackCoolDown = 1;
    float attackCooldownCounter = 0;
    public float attackDamage = 3f;
    public AudioClip attack;

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

        if (canAttack)
        {
            attackCooldownCounter += Time.deltaTime;
        }
    }

    public void RayHit()
    {
        DebugRay();

        if (Physics.Raycast(raycast, out hit))
        {
            //Debug.Log("Raycast: " + hit.collider.gameObject.name);

            if (canAttack && attackCooldownCounter >= attackCoolDown)
            {
                Pawn p = hit.collider.GetComponent<Pawn>();
                p.TakeDamage(this, attackDamage);
                Debug.Log(p.name);
                AudioSource.PlayClipAtPoint(attack, transform.position, 0.5f);
                attackCooldownCounter = 0;
            }

            if (hit.distance <= attackRange)
            {
                canMove = false;
                canAttack = true;
            }
            if (hit.distance >= 5)
            {
                canMove = true;
                canAttack = false;
            }
        }
    }

    public void DebugRay()
    {
        raycast.origin = this.transform.position;
        raycast.direction = this.transform.forward;

        Debug.DrawRay(raycast.origin, raycast.direction * hit.distance, Color.blue);
    }

    public override void CauseDeath()
    {
        base.CauseDeath();
    }
}