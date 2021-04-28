using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPawn : Pawn
{
	public Rigidbody rb;
	public Ray raycast;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	public override void Update()
	{
		base.Update();

		if (Health <= 0)
		{
			OnDeath();
		}
	}

	protected override void OnDeath()
	{
		Destroy(this.gameObject);
	}

}
