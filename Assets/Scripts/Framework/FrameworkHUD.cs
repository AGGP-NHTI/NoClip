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
	private HudReferences hud;
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

	//HUD objects color variables -- Find way to make list of objects referenced by name?
	[Header("---------- HUD Colors ----------")]
	[Header("Generic")]
	public Color criticalBaseColor = new Color(255, 255, 0, 255);
	public Color warningBaseColor = new Color(255, 0, 0, 255);
	public Color warningChangeColor = new Color(200, 0, 41, 255);

	[Header("Health")]
	public Color healthBaseColor = new Color(0, 152, 255, 255);
	public Color timerBaseColor = new Color(119, 155, 255, 255);
	public Color healthBackgroundColor = new Color(107, 106, 250, 100);
	public Color healthBoarderColor = new Color(25, 87, 255, 175);

	[Header("Radar")]
	public Color radarPlayerIconColor = new Color(107, 106, 250, 255);
	public Color radarBoarderColor = new Color(107, 106, 250, 200);
	public Color radarInnerBoarderColor = new Color(107, 106, 250, 150);
	public Color radarDividersColor = new Color(107, 106, 250, 140);
	public Color radarBackgroundColor = new Color(107, 106, 250, 100);
	public Color radarNotchColor = new Color(255, 0, 0, 200);
	public Color waveTextColor = new Color(119, 155, 255, 255);
	public Color waveBoarderColor = new Color(107, 106, 250, 200);
	public Color waveBackgroundColor = new Color(107, 106, 250, 100);

	[Header("Abilities")]
	public Color abilityBackgroundColor = new Color(107, 106, 250, 100);
	public Color abilityIconBoarderColor = new Color(107, 106, 250, 200);
	public Color abilityBarFillColor = new Color(0, 152, 255, 255);
	public Color abilityBarBoarderColor = new Color(107, 106, 250, 200);
	public Color abilityBarBackgroundColor = new Color(107, 106, 250, 100);
	public Color abilityMXBackgroundColor = new Color(107, 106, 250, 127);
	public Color abilityMXBoarderColor = new Color(107, 106, 250, 200);
	public Color abilityMXTextColor = new Color(119, 155, 255, 255);

	[Header("Information")]
	public Color scoreTextColor = new Color(119, 155, 255, 255);
	public Color multiplierTextColor = new Color(119, 155, 255, 255);
	public Color killsTextColor = new Color(119, 155, 255, 255);
	public Color streakTextColor = new Color(119, 155, 255, 255);

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
				hud = null;
			}
		}

		//If player is not a spectator
		if (!controller.GetPawn().IsSpectator)
		{
			if (currentPawnHUD == null)
			{
				Destroy(currentSpecHUD);
				currentPawnHUD = Instantiate(PawnHUD, HUDSpace.transform);

				hud = currentPawnHUD.GetComponentInChildren<HudReferences>();

				foreach (GameObject notch in hud.RadarNotches)
				{
					mc.RadarNotches.Add(notch);
				}
			}

			//To prevent not having an instance playerPawn error
			if (playerPawn != null)
			{
				//Get and update HUD colors
				hud = currentPawnHUD.GetComponentInChildren<HudReferences>();

				hud.AbilityBackground.color = abilityBackgroundColor;
				hud.AbilityBarBackground.color = abilityBarBackgroundColor;
				hud.AbilityBarBoarder.color = abilityBarBoarderColor;
				hud.AbilityBarFill.color = abilityBarFillColor;
				hud.AbilityIconBoarder.color = abilityIconBoarderColor;
				hud.AbilityMXBackground.color = abilityMXBackgroundColor;
				hud.AbilityMXBoarder.color = abilityMXBoarderColor;
				hud.AbilityMXText.color = abilityMXTextColor;
				hud.HealthBarBackground.color = healthBackgroundColor;
				hud.HealthBarBoarder.color = healthBoarderColor;
				hud.PlayerIcon.color = radarPlayerIconColor;
				hud.RadarBackground.color = radarBackgroundColor;
				hud.RadarBoarder.color = radarBoarderColor;
				hud.RadarDividers.color = radarDividersColor;
				hud.RadarInnerBoarder.color = radarInnerBoarderColor;
				hud.WaveBackground.color = waveBackgroundColor;
				hud.WaveBoarder.color = waveBoarderColor;
				hud.WaveText.color = waveTextColor;
				hud.ScoreText.color = scoreTextColor;
				hud.MultiplierText.color = multiplierTextColor;
				hud.KillsText.color = killsTextColor;
				hud.StreakText.color = streakTextColor;

				for (int i = 0; i < mc.RadarNotches.Count; i++)
				{
					Image notch = mc.RadarNotches[i].GetComponent<Image>();
					notch.color = radarNotchColor;
				}

				//Get a float for setting healthbar fill amount
				float healthPercentage = playerPawn.Health / playerPawn.StartingHealth;

				//Set fill amount
				hud.HealthBars[0].fillAmount = healthPercentage;
				hud.HealthBars[1].fillAmount = healthPercentage;

				//Set health time text to current health with 2 decimal places ("F2")
				hud.HealthTimer.text = playerPawn.Health.ToString("F2");

				if (playerPawn.Health <= (playerPawn.StartingHealth * playerPawn.criticalLevel) && playerPawn.Health > (playerPawn.StartingHealth * playerPawn.warningLevel))
				{
					hud.HealthWarningSymbol.gameObject.SetActive(true);

					hud.HealthWarningSymbol.color = criticalBaseColor;
					hud.HealthTimer.color = criticalBaseColor;
					hud.HealthBars[0].color = criticalBaseColor;
					hud.HealthBars[1].color = criticalBaseColor;
				}
				//For displaying a warning when the player is under set seconds worth of health
				else if (playerPawn.Health <= (playerPawn.StartingHealth * playerPawn.warningLevel))
				{
					warningTextColorSwap += Time.deltaTime;

					if (warningTextColorSwap > swapWarningTextColor && warningTextColorSwap < resetWarningTextColor)
					{
						hud.HealthWarningSymbol.gameObject.SetActive(true);

						hud.HealthWarningSymbol.color = warningBaseColor;
						hud.HealthTimer.color = warningBaseColor;
						hud.HealthBars[0].color = warningBaseColor;
						hud.HealthBars[1].color = warningBaseColor;
					}
					else if (warningTextColorSwap > resetWarningTextColor)
					{
						hud.HealthWarningSymbol.color = warningChangeColor;
						hud.HealthTimer.color = warningChangeColor;
						hud.HealthBars[0].color = warningChangeColor;
						hud.HealthBars[1].color = warningChangeColor;
						warningTextColorSwap = 0f;
					}
				}
				else 
				{
					hud.HealthBars[0].color = healthBaseColor;
					hud.HealthBars[1].color = healthBaseColor;
					hud.HealthTimer.color = timerBaseColor;
					hud.HealthWarningSymbol.color = warningBaseColor;
					warningTextColorSwap = 0f;
					hud.HealthWarningSymbol.gameObject.SetActive(false);
				}
			}
		}
	}

	private void LateUpdate()
	{
		UpdateHUD();
	}
}
