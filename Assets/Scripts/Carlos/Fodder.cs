using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fodder : EnemyPawn
{
	public bool canMove = true;

	RaycastHit hit;

	float distanceToTarget;
	public float damage = 20f;
	public float explosionRadius = 3f;
	public List<Actor> actors;
	bool explode = false;

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
	}

	public void RayHit()
	{
		DebugRay();

		if (Physics.Raycast(raycast, out hit))
		{
			//Debug.Log("Raycast: " + hit.collider.gameObject.name);

			if (hit.distance <= explosionRadius / 2)
			{
				canMove = false;

				if (!explode)
				{
					Explode();
				}
			}
		}
	}

	public void DebugRay()
	{
		raycast.origin = this.transform.position;
		raycast.direction = this.transform.forward;

		Debug.DrawRay(raycast.origin, raycast.direction * hit.distance, Color.red);
	}

	public void Explode()
	{
		explode = true;

		actors.AddRange(FindObjectsOfType<Actor>());

		actors.Remove(this);

		foreach (Actor a in actors)
		{
			//Find distance to target
			distanceToTarget = Vector3.Distance(a.gameObject.transform.position, transform.position);

			//If target is within damage range
			if (distanceToTarget <= explosionRadius)
			{
				Pawn p = a.gameObject.GetComponent<Pawn>();
				if (p)
				{
					p.TakeDamage(this, damage);
				}
			}

			Health -= 10000f;
		}
	}
}