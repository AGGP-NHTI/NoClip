using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamageType : BaseDamageType
{
	public override void Initialize()
	{   
		verb = "hits";
		Instakill = false;
		CausedByWorld = false;
		IsPointDamage = true;
		IsRadialDamage = false;
		DoesFireDamage = false;
		DoesExplosiveDamage = false;
		DoesPiercingDamage = false;
	}
}
