using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		PlayerPawn pc = other.GetComponent<PlayerPawn>();

		if(pc)
		{
			pc.CauseDeath();
		}
	}
}
