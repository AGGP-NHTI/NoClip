using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Game : Info
{
	public static Game Instance;

	public GameObject SpawnerObject; //In the chance there are no Spawnpoints in world
	public Spawnpoint[] Spawnpoints; //Array of Spawnpoints already in world
	int spawnIndex;
	Spawnpoint ChosenSpawn; //Used in ReturnSpawn when Spectator is requesting a spawn location for new Pawn
	[Header("Stats for Current Run")]
	public float score = 0;
	public float scoreMX = 1.0f;
	public int kills = 0;
	public int streak = 0;
	[Header("Highest Dynamic Stats for Current Run")]
	public int highStreakRun = 0;
	public float highSMXRun = 0;

	private float highScore;
	private float highScoreMX;
	private int highKills;
	private int highStreak;
	public TextMeshProUGUI[] textFields;

	//Singleton Instance of Class
	private void Awake()
	{
		Instance = this;

		highStreak = PlayerPrefs.GetInt("HighStreak");
		highKills = PlayerPrefs.GetInt("HighKills");
		highScore = PlayerPrefs.GetFloat("HighScore");
		highScoreMX = PlayerPrefs.GetFloat("HighSMX");

		if (textFields.Length >= 1)
		{
			highStreakRun = PlayerPrefs.GetInt("Streak");
			kills = PlayerPrefs.GetInt("Kills");
			score = PlayerPrefs.GetFloat("Score");
			highSMXRun = PlayerPrefs.GetFloat("SMX");
			textFields[0].text = score.ToString("F1");
			textFields[1].text = highSMXRun.ToString("F1");
			textFields[2].text = kills.ToString();
			textFields[3].text = highStreakRun.ToString();
			textFields[4].text = highScore.ToString("F1");
			textFields[5].text = highScoreMX.ToString("F1");
			textFields[6].text = highKills.ToString();
			textFields[7].text = highStreak.ToString();
		}
		UpdateHighStats();
	}

	void Start()
	{
		Spawnpoints = GameObject.FindObjectsOfType(typeof(Spawnpoint)) as Spawnpoint[];

		if (Spawnpoints.Length == 0)
		{
			Debug.LogError("No Spawnpoints found!");
			Debug.LogWarning("Creating Default Point at World Center");

			Factory(SpawnerObject, Vector3.zero, Rotation);
		}
	}

	public void Update()
	{
		UpdateHighRun();
	}

	public Spawnpoint ReturnSpawn()
	{
		spawnIndex = Random.Range(0, Spawnpoints.Length - 1);

		ChosenSpawn = Spawnpoints[spawnIndex];

		return ChosenSpawn;
	}

	public void UpdateHighRun()
	{
		if (streak > highStreakRun)
		{
			highStreakRun = streak;
		}

		if (scoreMX > highSMXRun)
		{
			highSMXRun = scoreMX;
		}
	}

	public void UpdateHighStats()
	{
		if (highStreakRun > highStreak)
		{
			highStreak = highStreakRun;
			PlayerPrefs.SetInt("HighStreak", highStreak);
			//Debug.Log("Updated Highest Streak");
		}

		if (kills > highKills)
		{
			highKills = kills;
			PlayerPrefs.SetInt("HighKills", highKills);
			//Debug.Log("Updated Highest Kills");
		}

		if (score > highScore)
		{
			highScore = score;
			PlayerPrefs.SetFloat("HighScore", highScore);
			//Debug.Log("Updated Highest Score");
		}

		if (highSMXRun > highScoreMX)
		{
			highScoreMX = highSMXRun;
			PlayerPrefs.SetFloat("HighSMX", highScoreMX);
			//Debug.Log("Updated Highest ScoreMX");
		}
	}

	public void ResetHighStats()
	{
		Debug.Log("Resetting Best Stats");
		highStreak = 0;
		highKills = 0;
		highScore = 0;
		highScoreMX = 1;

		PlayerPrefs.SetInt("HighStreak", highStreak);
		PlayerPrefs.SetInt("HighKills", highKills);
		PlayerPrefs.SetFloat("HighScore", highScore);
		PlayerPrefs.SetFloat("HighSMX", highScoreMX);
		textFields[4].text = "0";
		textFields[5].text = "1";
		textFields[6].text = "0";
		textFields[7].text = "0";
		SaveStats();
	}

	public void ResetStats()
	{
		Debug.Log("Clear Stats for this run");
		streak = 0;
		kills = 0;
		score = 0;
		scoreMX = 1;
		highStreakRun = 0;
		highSMXRun = 0;

		PlayerPrefs.SetInt("Streak", streak);
		PlayerPrefs.SetInt("Kills", kills);
		PlayerPrefs.SetFloat("Score", score);
		PlayerPrefs.SetFloat("SMX", scoreMX);
		textFields[0].text = "0";
		textFields[1].text = "1";
		textFields[2].text = "0";
		textFields[3].text = "0";
		SaveStats();
	}

	public void SaveStats()
	{
		PlayerPrefs.SetInt("Streak", highStreakRun);
		PlayerPrefs.SetInt("Kills", kills);
		PlayerPrefs.SetFloat("Score", score);
		PlayerPrefs.SetFloat("SMX", highSMXRun);

		PlayerPrefs.SetInt("HighStreak", highStreak);
		PlayerPrefs.SetInt("HighKills", highKills);
		PlayerPrefs.SetFloat("HighScore", highScore);
		PlayerPrefs.SetFloat("HighSMX", highScoreMX);

		PlayerPrefs.Save();
	}
}
