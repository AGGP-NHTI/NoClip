using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : EnemyPawn
{
	public bool canMove = false;
	public bool canAttack = false;
	public float attackCoolDown = 1;
	public float attackCooldownCounter = 0;
	public float attackDamage = 3f;
	public float attackRange = 2f;
	public AudioClip attack;

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
			Debug.Log("Raycast: " + hit.collider.gameObject.name);

			if (canAttack && attackCooldownCounter >= attackCoolDown)
			{
				Pawn p = hit.collider.GetComponent<PlayerPawn>();
				p.TakeDamage(this, attackDamage);
				AudioSource.PlayClipAtPoint(attack, transform.position, 0.5f);
				attackCooldownCounter = 0;
			}

			if (hit.distance <= 10)
			{
				//speed = 3;
			}
			if (hit.distance <= attackRange)
			{
				canMove = false;
				canAttack = true;
			}
			if (hit.distance > 10)
			{
				canAttack = false;
				canMove = true;
				//moveSpeed = 10;
			}
		}
	}

	public void DebugRay()
	{
		raycast.origin = this.transform.position;
		raycast.direction = this.transform.forward;

		Debug.DrawRay(raycast.origin, raycast.direction * hit.distance, Color.yellow);
	}
}
