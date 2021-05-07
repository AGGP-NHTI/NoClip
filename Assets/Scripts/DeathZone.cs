using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		Pawn pc = other.GetComponent<Pawn>();

		if(pc)
		{
			pc.CauseDeath();
		}
	}
}
