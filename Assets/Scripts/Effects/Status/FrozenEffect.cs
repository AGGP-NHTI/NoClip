using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenEffect : MonoBehaviour
{
	public static void OnUpdate_FrozenEffect(Pawn p, BaseEffect frozen)
	{
		BaseEffect Effect = p.OngoingEffects.Find(effect => effect.Name == "Frozen");

		float freezePercentage;

		if (Effect == null)
		{
			p.AddOngingEffect(frozen);

			frozen.OnUpdate += (pawn, time) =>
			{
				freezePercentage = frozen.CurrentDuration / frozen.MaxDuration;

				pawn.UpdateEffectHUD(frozen, freezePercentage);

				int numTicksLeft = Mathf.FloorToInt(frozen.CurrentDuration);
				frozen.CurrentDuration -= time;

				if (frozen.CurrentDuration <= 0)
				{
					pawn.RemoveOngingEffect(frozen);
					frozen.CurrentDuration = 0;
					Destroy(frozen);
				}
			};
		}
		else
		{
			frozen.CurrentDuration = frozen.MaxDuration;
		}
	}

	public static void OnBegin_FrozenEffect(Pawn p, BaseEffect frozen)
	{
		p.AddEffectHUD(frozen);
		p.moveSpeed /= frozen.Amount;
		p.ParticleGenerators[1].gameObject.SetActive(true);
	}

	public static void OnEnd_FrozenEffect(Pawn p, BaseEffect frozen)
	{
		p.RemoveEffectHUD(frozen);
		p.moveSpeed *= frozen.Amount;
		p.ParticleGenerators[1].gameObject.SetActive(false);
	}
}
