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
	public bool canMove = false;
	public BlockingDoor door;
	Game game;
	protected PlayerPawn pp;

	public virtual void Start()
	{
		rb = GetComponent<Rigidbody>();
		game = FindObjectOfType<Game>();
		canMove = false;
	}

	public override void Update()
	{
		base.Update();

		if (!pp)
		{
			pp = FindObjectOfType<PlayerPawn>();
		}

		if (Health <= 0)
		{
			OnDeath();
		}
	}

	public virtual void PerformAttack()
	{

	}

	protected override bool ProcessDamage(Actor Source, float Value, DamageEventInfo EventInfo, Controller Instigator)
	{
		PlayerPawn p = FindObjectOfType<PlayerPawn>();
		if (p.siphonActive)
		{
			p.PlaySiphon();
		}
		return base.ProcessDamage(Source, Value, EventInfo, Instigator);
	}

	protected override void OnDeath()
	{
		game.score += scoreOnKill * game.scoreMX;
		game.scoreMX += scoreOnKill / 100f;
		game.kills++;
		game.streak++;

		//If the players streak is better than the current streak on this run
		if (game.streak >= game.highStreakRun)
		{
			game.highStreakRun++;
		}

		//If the player gets a score multiplier that is better than the current scoreMX on this run
		if (game.scoreMX >= game.highSMXRun)
		{
			game.highSMXRun = game.scoreMX;
		}

		PlayerPawn p = FindObjectOfType<PlayerPawn>();

		if (p.lastAttackWasMelee)
		{
			p.Health += energyOnDeath;
		}

		isDead = true;

		if(door)
		{
			door.RemoveEnemy(this);
		}

		Destroy(this.gameObject);
	}
}
