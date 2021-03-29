using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spectator : Pawn
{
	private void Start()
	{
		IsSpectator = true;
		IgnoresDamage = true;
	}

	public override void Fire1(bool value)
	{
		//Debug.Log(this.name + " - Spectator: Fire1");

		//Calls Controller RequestSpawn
		controller.RequestSpawn();
	}
}
