using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Actor
{
	[Header("General Stats")]
	public float lifetime = 5;
	float lifeTic = 0;
	float tempTic = 0;
	public float damage = 10f;
	public float explosionRadius = 3f;

	[Header("Status Effects")]
	BaseEffect fireburn;
	public BaseEffect FireBurnEffectReference;

	[Header("Particles")]
	public ParticleSystem particleFireball;
	public ParticleSystem particleImpact;

	[Header("Audio")]
	public AudioSource audioSource;
	public AudioClip explosionOneShot;

	float distanceToTarget;
	public List<Actor> actors;
	bool impacted = false;

	void Update()
	{
		lifeTic += Time.deltaTime;

		//If the fireball exceeds its lifetime
		if (lifeTic >= lifetime && impacted == false)
		{
			Expire();
		}

		//If the fireball impacts an object
		if (lifeTic > tempTic + 2 && impacted == true)
		{
			Destroy(this.gameObject);
		}

	}

	private void OnTriggerEnter(Collider other)
	{
		if (lifeTic > 0.011)
		{
			if (other.gameObject.tag == "Ground")
			{
				if (impacted == false)
				{
					Impact();
				}
			}

			if (other.gameObject.GetComponent<Pawn>())
			{
				Pawn p = other.gameObject.GetComponent<Pawn>();

				if (p)
				{
					BaseEffect Effect = p.OngoingEffects.Find(effect => effect.Name == "Fire Burn");

					if (Effect == null)
					{
						fireburn = fireburn.NewInstance(FireBurnEffectReference, p.gameObject);
						fireburn.overTime += (effectDamage, effectDuration) => FireBurnEffect.OnUpdate_FireBurnEffect(p, fireburn);
						fireburn.OnBeginEffect += (effect) => FireBurnEffect.OnBegin_FireBurnEffect(p, fireburn);
						fireburn.OnEndEffect += (effect) => FireBurnEffect.OnEnd_FireBurnEffect(p, fireburn);

						fireburn.EffectUpdate(fireburn.Amount, fireburn.CurrentDuration);
					}
					else
					{
						Effect.overTime += (effectDamage, effectDuration) => FireBurnEffect.OnUpdate_FireBurnEffect(p, Effect);
						Effect.EffectUpdate(Effect.Amount, Effect.MaxDuration);
					}
				}

				if (impacted == false)
				{
					Impact();
				}
			}
		}
	}

	private void Impact()
	{
		actors.AddRange(FindObjectsOfType<Actor>());
		audioSource.PlayOneShot(explosionOneShot);

		foreach (Actor a in actors)
		{
			distanceToTarget = Vector3.Distance(a.gameObject.transform.position, this.gameObject.transform.position);

			if (distanceToTarget <= explosionRadius)
			{
				a.TakeDamage(this, damage, Owner);
				new DamageEventInfo(typeof(FireballDamageType));
			}
		}

		particleFireball.Stop();
		particleImpact.Play();
		impacted = true;

		tempTic = lifeTic;
	}

	private void Expire()
	{
		particleFireball.Stop();

		if (lifeTic > lifetime + 1)
		{
			Destroy(this.gameObject);
		}
	}
}
