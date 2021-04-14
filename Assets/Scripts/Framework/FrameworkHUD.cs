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
	public Controller controller;

	private MinimapController mc;
	private HudReferences hr;
	private Pawn pawn;
	private GameObject HUDSpace;
	private GameObject PawnHUD;
	private GameObject SpectatorHUD;
	private PlayerController[] pc;
	private PlayerPawn playerPawn;
	private GameObject currentSpecHUD = null;
	private GameObject currentPawnHUD = null;

	private float warningTextColorSwap = 0f;
	public float swapWarningTextColor = 0.25f;
	public float resetWarningTextColor = 0.5f;
	public Color baseColor;
	public Color changeColor;

	public void Start()
	{
		pc = controller.GetComponents<PlayerController>();
		mc = controller.GetComponent<MinimapController>();

		//For player 1
		if (controller.PlayerNumber == 1)
		{
			HUDSpace = pc[0].HUDSpace;
			PawnHUD = pc[0].PawnHUD;
			SpectatorHUD = pc[0].SpectatorHUD;
		}
	}

	public void Update()
	{

		//Temporary key inputs to change health
		{
			if (Input.GetKeyDown(KeyCode.Y))
			{
				playerPawn.Health -= 10f;
			}
			if (Input.GetKeyDown(KeyCode.H))
			{
				playerPawn.Health += 10f;
			}
		}

		//If player dies or respawns get new instance of the pawn
		pawn = controller.GetPawn();
		playerPawn = pawn.GetComponent<PlayerPawn>();
	}

	/// <summary>
	/// Player Controller Calls this Method to update the Player's HUD
	/// </summary>
	public virtual void UpdateHUD()
	{
		//If player is a spectator
		if (controller.GetPawn().IsSpectator)
		{
			if (currentSpecHUD == null)
			{
				Destroy(currentPawnHUD);
				currentSpecHUD = Instantiate(SpectatorHUD, HUDSpace.transform);
				hr = null;
			}
		}

		//If player is not a spectator
		if (!controller.GetPawn().IsSpectator)
		{
			if (currentPawnHUD == null)
			{
				Destroy(currentSpecHUD);
				currentPawnHUD = Instantiate(PawnHUD, HUDSpace.transform);

				hr = currentPawnHUD.GetComponentInChildren<HudReferences>();

				foreach (GameObject notch in hr.RadarNotches)
				{
					mc.RadarNotches.Add(notch);
				}
			}

			//To prevent not having an instance playerPawn error
			if (playerPawn != null)
			{
				//Get a float for setting healthbar fill amount
				float healthPercentage = playerPawn.Health / playerPawn.StartingHealth;

				//Set fill amount
				hr.HealthBars[0].fillAmount = healthPercentage;
				hr.HealthBars[1].fillAmount = healthPercentage;

				//Set health time text to current health
				hr.HealthTimer.text = playerPawn.Health.ToString("F2");

				//For displaying a warning when the player is under 10 seconds worth of health
				if (playerPawn.Health <= 10f)
				{
					hr.HealthWarningText.gameObject.SetActive(true);
					hr.HealthWarningSymbol.gameObject.SetActive(true);

					warningTextColorSwap += Time.deltaTime;

					if (warningTextColorSwap > swapWarningTextColor && warningTextColorSwap < resetWarningTextColor)
					{
						hr.HealthWarningText.color = baseColor;
						hr.HealthWarningSymbol.color = baseColor;
					}
					else if (warningTextColorSwap > resetWarningTextColor)
					{
						hr.HealthWarningText.color = changeColor;
						hr.HealthWarningSymbol.color = changeColor;
						warningTextColorSwap = 0f;
					}
				}
				else
				{
					hr.HealthWarningText.color = baseColor;
					hr.HealthWarningSymbol.color = baseColor;
					warningTextColorSwap = 0f;
					hr.HealthWarningText.gameObject.SetActive(false);
					hr.HealthWarningSymbol.gameObject.SetActive(false);
				}
			}
		}
	}

	private void LateUpdate()
	{
		UpdateHUD();
	}
}
