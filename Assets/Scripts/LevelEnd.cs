using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
	Game game;

	private void Awake()
	{
		game = FindObjectOfType<Game>();
	}

	private void OnTriggerEnter(Collider other)
	{
		PlayerPawn pc = other.GetComponent<PlayerPawn>();

		if(pc)
		{
			EndLevel();
		}
	}

	public void EndLevel()
	{
		game.SaveStats();
		SceneManager.LoadScene(2);
	}
}
