using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// HUD Script Class is attached to a Pannel in a Prefab
/// </summary>
public class FrameworkHUD : Actor
{
	public void Start()
	{
	
	}

	public void Update()
	{
		
	}

	/// <summary>
	/// Player Controller Calls this Method to update the Player's HUD
	/// </summary>
	public virtual void UpdateHUD()
	{
	
	}

	private void LateUpdate()
	{
		UpdateHUD();
	}
}
