using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBurnEffect : MonoBehaviour
{
	public static void OnUpdate_FireBurnEffect(Pawn p, BaseEffect burn)
	{
		BaseEffect Effect = p.OngoingEffects.Find(effect => effect.Name == "Fire Burn");

		float damage = burn.Amount;
		float burnPercentage;

		if (Effect == null)
		{
			p.AddOngingEffect(burn);

			burn.OnUpdate += (pawn, time) =>
			{
				burnPercentage = burn.CurrentDuration / burn.MaxDuration;

				pawn.UpdateEffectHUD(burn, burnPercentage);

				int numTicksLeft = Mathf.FloorToInt(burn.CurrentDuration);
				burn.CurrentDuration -= time;

				if (Mathf.FloorToInt(burn.CurrentDuration) < numTicksLeft)
				{
					pawn.TakeDamage(burn.Source, damage);
				}

				if (burn.CurrentDuration <= 0)
				{
					pawn.RemoveOngingEffect(burn);
					burn.CurrentDuration = 0;
					Destroy(burn);
				}
			};
		}
		else
		{
			burn.CurrentDuration = burn.MaxDuration;
		}
	}

	public static void OnBegin_FireBurnEffect(Pawn p, BaseEffect burn)
	{
		p.AddEffectHUD(burn);
		p.ParticleGenerators[0].gameObject.SetActive(true);
	}

	public static void OnEnd_FireBurnEffect(Pawn p, BaseEffect burn)
	{
		p.RemoveEffectHUD(burn);
		p.ParticleGenerators[0].gameObject.SetActive(false);
	}
}
