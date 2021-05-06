using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fodder : EnemyPawn
{
	RaycastHit hit;

	float distanceToTarget;
	public float damage = 20f;
	public float explosionRadius = 3f;
	public List<Actor> actors;
	public AudioClip explosion;
	public ParticleSystem ps;
	bool explode = false;
	float explodeTime = 0f;
	public float deathTime = 5f;
	public MeshRenderer[] renderers;

	public override void Update()
	{
		base.Update();

		if (explode)
		{
			explodeTime += Time.deltaTime;
		}

		if (explodeTime > deathTime)
		{
			Destroy(this.gameObject);
		}

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

		foreach (GameObject go in ParticleGenerators)
		{
			go.SetActive(false);
		}	

		for (int i = 0; i < renderers.Length; i++)
		{
			renderers[i].enabled = false;
		}

		AudioSource.PlayClipAtPoint(explosion, gameObject.transform.position, 0.25f);
		ps.Play();

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
		}
	}

	protected override void OnDeath()
	{
		if (!explode)
		{
			Explode();
		}
	}
}