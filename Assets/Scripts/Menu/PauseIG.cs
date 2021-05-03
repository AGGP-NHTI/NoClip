using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseIG : MonoBehaviour
{
	public bool gameIsPaused = false;

	public GameObject pauseMenuUI;
	public GameObject settingsMenu;
	FrameworkHUD FHUD;

	public int pauseIndex = 0;

	public void Awake()
	{
		if (FindObjectOfType<FrameworkHUD>())
		{
			FHUD = FindObjectOfType<FrameworkHUD>();
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown("escape"))
		{
			Debug.Log("Escape Pressed");
			if (gameIsPaused)
			{
				Resume();
			}
			else
			{
				Pause();
			}

			if (pauseIndex == 0)
			{
				gameIsPaused = false;
			}
			else if (pauseIndex == 1)
			{
				gameIsPaused = true;
			}
			else if (pauseIndex == 2)
			{
				pauseIndex = 1;
				settingsMenu.SetActive(false);
				pauseMenuUI.SetActive(true);
			}
		}
	}

	public void Resume()
	{
		if (pauseIndex == 1)
		{
			Debug.Log("Resume");
			pauseMenuUI.SetActive(false);
			FHUD.currentPawnHUD.SetActive(true);
			Time.timeScale = 1f;
			pauseIndex = 0;
			gameIsPaused = false;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	public void Pause()
	{
		Debug.Log("Pause");
		pauseMenuUI.SetActive(true);
		FHUD.currentPawnHUD.SetActive(false);
		Time.timeScale = 0f;
		pauseIndex = 1;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	public void IncreaseIndexOne()
	{
		pauseIndex++;
	}

	public void DecreaseIndexOne()
	{
		pauseIndex--;
	}
}
