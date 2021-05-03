using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : Actor
{
	[Header("General Stats")]
	public float lifetime = 5;
	float lifeTic = 0;
	public float damage = 10f;
	public float freezeRadius = 3f;

	[Header("Status Effects")]
	BaseEffect frozen;
	public BaseEffect FreezeEffectReference;

	[Header("Particles")]
	public ParticleSystem particleFreeze;

	[Header("Audio")]
	public AudioSource audioSource;
	public AudioClip freezeOneShot;

	float distanceToTarget;
	public List<Actor> actors;
	public Actor owner;
	public Controller contOwner;

	void Update()
	{
		lifeTic += Time.deltaTime;

		//If the freeze exceeds its lifetime
		if (lifeTic >= lifetime)
		{
			Expire();
		}
	}

	public void Activate()
	{
		actors.AddRange(FindObjectsOfType<Actor>());

		foreach (Actor a in actors)
		{
			//Find distance to target
			distanceToTarget = Vector3.Distance(a.gameObject.transform.position, this.gameObject.transform.position);

			//If target is within freeze range
			if (distanceToTarget <= freezeRadius)
			{
				Pawn p = a.gameObject.GetComponent<Pawn>();

				if (p)
				{
					BaseEffect Effect = p.OngoingEffects.Find(effect => effect.Name == "Frozen");

					//If the target is not the caster
					if (p && p.GetController() != contOwner)
					{
						p.TakeDamage(owner, damage, contOwner);
						//new DamageEventInfo(typeof(FreezeDamageType));

						if (Effect == null)
						{
							frozen = frozen.NewInstance(FreezeEffectReference, p.gameObject);
							frozen.overTime += (effectDamage, effectDuration) => FrozenEffect.OnUpdate_FrozenEffect(p, frozen);
							frozen.OnBeginEffect += (effect) => FrozenEffect.OnBegin_FrozenEffect(p, frozen);
							frozen.OnEndEffect += (effect) => FrozenEffect.OnEnd_FrozenEffect(p, frozen);

							frozen.EffectUpdate(frozen.Amount, frozen.CurrentDuration);
						}
						else
						{
							Effect.overTime += (effectDamage, effectDuration) => FrozenEffect.OnUpdate_FrozenEffect(p, Effect);
							Effect.EffectUpdate(Effect.Amount, Effect.MaxDuration);
						}
					}
				}
			}
		}

		actors.Clear();
	}

	private void Expire()
	{
		if (lifeTic > lifetime + 1)
		{
			Destroy(this.gameObject);
		}
	}
}
