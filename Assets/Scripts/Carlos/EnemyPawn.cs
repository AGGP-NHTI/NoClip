using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPawn : Pawn
{
	public Rigidbody rb;
	public Ray raycast;
	public int scoreOnKill;
	public float energyOnHit;
	public float energyOnDeath;
	public bool isDead = false;
	Game game;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		game = FindObjectOfType<Game>();
	}

	public override void Update()
	{
		base.Update();

		if (Health <= 0)
		{
			OnDeath();
		}
	}

	public virtual void PerformAttack()
	{

	}

	protected override void OnDeath()
	{
		game.score += scoreOnKill * game.scoreMX;
		game.scoreMX += scoreOnKill / 100f;
		game.kills++;
		game.streak++;

		PlayerPawn p = FindObjectOfType<PlayerPawn>();
		
		if (p.lastAttackWasMelee)
		{
			p.Health += energyOnDeath;
		}

		isDead = true;

		Destroy(this.gameObject);
	}
}
