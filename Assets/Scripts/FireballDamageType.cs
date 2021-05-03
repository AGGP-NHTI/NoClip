using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballDamageType : BaseDamageType
{
	public override void Initialize()
	{
		verb = "explodes";
		Instakill = false;
		CausedByWorld = false;
		IsPointDamage = false;
		IsRadialDamage = true;
		DoesFireDamage = true;
		DoesExplosiveDamage = true;
		DoesPiercingDamage = false;
	}
}
