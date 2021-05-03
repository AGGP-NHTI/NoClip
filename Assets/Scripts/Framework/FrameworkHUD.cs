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

	private Game game;
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

	[Header("Radial Menu")]
	public GameObject theMenu;

	public Image[] options;

	public Color normalColor, highlightedColor;

	public int selectedOption;
	private int hoverOption;

	public Vector2 moveInput;
	private int angleSegment;
	private float menuWait = 0f;


	//HUD objects color variables -- Find way to make list of objects referenced by name?
	[Header("---------- HUD Colors ----------")]
	[Header("Generic")]
	public Color lowBaseColor = new Color(255, 255, 0, 255);
	public Color criticalBaseColor = new Color(255, 0, 0, 255);
	public Color criticalChangeColor = new Color(200, 0, 41, 255);

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
		game = controller.GetComponent<Game>();

		angleSegment = 360 / options.Length;

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

		if (theMenu.activeInHierarchy == true)
		{
			menuWait = 20;
		}

		if (menuWait > 0)
		{
			menuWait--;
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
				UpdateHUDColor();

				hud.AbilityBarFill.fillAmount = playerPawn.getEnergy;
				hud.AbilityMXText.text = "X" + playerPawn.getEnergy.ToString("F1");

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

				if (playerPawn.Health <= (playerPawn.StartingHealth * playerPawn.lowLevel) && playerPawn.Health > (playerPawn.StartingHealth * playerPawn.criticalLevel))
				{
					hud.HealthWarningSymbol.gameObject.SetActive(true);

					hud.HealthWarningSymbol.color = lowBaseColor;
					hud.HealthTimer.color = lowBaseColor;
					hud.HealthBars[0].color = lowBaseColor;
					hud.HealthBars[1].color = lowBaseColor;
				}
				//For displaying a warning when the player is under set seconds worth of health
				else if (playerPawn.Health <= (playerPawn.StartingHealth * playerPawn.criticalLevel))
				{
					warningTextColorSwap += Time.deltaTime;

					if (warningTextColorSwap > swapWarningTextColor && warningTextColorSwap < resetWarningTextColor)
					{
						hud.HealthWarningSymbol.gameObject.SetActive(true);

						hud.HealthWarningSymbol.color = criticalBaseColor;
						hud.HealthTimer.color = criticalBaseColor;
						hud.HealthBars[0].color = criticalBaseColor;
						hud.HealthBars[1].color = criticalBaseColor;
					}
					else if (warningTextColorSwap > resetWarningTextColor)
					{
						hud.HealthWarningSymbol.color = criticalChangeColor;
						hud.HealthTimer.color = criticalChangeColor;
						hud.HealthBars[0].color = criticalChangeColor;
						hud.HealthBars[1].color = criticalChangeColor;
						warningTextColorSwap = 0f;
					}
				}
				else 
				{
					hud.HealthBars[0].color = healthBaseColor;
					hud.HealthBars[1].color = healthBaseColor;
					hud.HealthTimer.color = timerBaseColor;
					hud.HealthWarningSymbol.color = criticalBaseColor;
					warningTextColorSwap = 0f;
					hud.HealthWarningSymbol.gameObject.SetActive(false);
				}
			}
		}
	}

	private void UpdateHUDColor()
	{
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
	}

	public void UpdateRadMenu()
	{
		if (playerPawn.abilityMenuOpen == true)
		{
			theMenu.SetActive(true);	
		}
		else
		{
			theMenu.SetActive(false);
		}

		if (theMenu.activeInHierarchy)
		{
			moveInput.x = Input.mousePosition.x - (Screen.width / 2f);
			moveInput.y = Input.mousePosition.y - (Screen.height / 2f);
			moveInput.Normalize();

			if (moveInput != Vector2.zero)
			{
				//Calculate Angle in degrees from 0 to 360
				float angle = Mathf.Atan2(moveInput.y, -moveInput.x) / Mathf.PI;
				angle *= 180; // Convert from pi to degrees
				angle += 90f; // Set 0 position
				if (angle < 0)
				{
					angle += 360; // Convert from -180 to 180 -> 0 to 360
				}

				for (int i = 0; i < options.Length; i++)
				{
					//if the angle is greater than the index low bound,
					//and the angle is less than the next index low bound
					if (angle > i * angleSegment && angle < (i + 1) * angleSegment)
					{
					//	options[i].color = highlightedColor;
						hoverOption = i;
					}
					else
					{
					//	options[i].color = normalColor;
					}
				}
			}
			if (Input.GetMouseButtonDown(0))
			{
				selectedOption = hoverOption;
				hud.AbilityIcon.sprite = options[selectedOption].sprite;
				playerPawn.index = selectedOption;
			}
		}
	}

	public void UpdateInfo()
	{
		hud.KillsText.text = "Kills:" + game.kills.ToString();
		hud.StreakText.text = "Streak:" + game.streak.ToString();
		hud.ScoreText.text = "Score:" + game.score.ToString();
		hud.MultiplierText.text = "Multiplier:" + game.scoreMX.ToString("F2");
	}

	private void LateUpdate()
	{
		UpdateHUD();
		UpdateRadMenu();
		UpdateInfo();
	}
}
