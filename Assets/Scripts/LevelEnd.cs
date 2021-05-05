using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{

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
		SceneManager.LoadScene(2);
	}
}
